using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.Records.Record.Commands;
using Hospital.Application.CQRS.Records.Views;
using Hospital.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Record = Hospital.Domain.Record;

namespace Hospital.Tests.RecordsTests
{
    public class RecordCommandTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IRecordCommand _iRecordCommand;
        private readonly IMapper _mapper;

        public RecordCommandTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _iRecordCommand = new RecordCommand(_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task CreateAsync_ShouldBeExpected()
        {
            var user = new List<User>
            {
                _fixture.Build<User>()
                    .Without(i => i.Records)
                    .Create()
            };

            await _hospitalContextFixture.InitUsersAsync(user);

            var patient = await _hospitalContextFixture.context.Users.FirstOrDefaultAsync();

            var doctorSchedule = patient.Doctor.Schedules.FirstOrDefault();

            var dateTimeUtcNow = DateTime.UtcNow;

            var dateTime = new DateTime(dateTimeUtcNow.Year, dateTimeUtcNow.Month, dateTimeUtcNow.Day,
                doctorSchedule.BeginWork.Hours, doctorSchedule.BeginWork.Minutes, doctorSchedule.BeginWork.Seconds);

            var bookedTime = Next(dateTime, doctorSchedule.DayOfWeek);

            var result = await _iRecordCommand.CreateAsync(Guid.Parse(patient.Id), patient.Doctor.DoctorId, bookedTime);

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateBookedTimeAsync_ShouldBeExpected()
        {
            var user = new List<User>
            {
                _fixture.Build<User>()
                    .Create()
            };

            await _hospitalContextFixture.InitUsersAsync(user);

            var patient = await _hospitalContextFixture.context.Users.FirstOrDefaultAsync();

            var record = patient.Records.FirstOrDefault();

            var oldRecordbookedTime = record.BookedTime;

            var doctorSchedule = patient.Doctor.Schedules.FirstOrDefault();

            var dateTimeUtcNow = record.BookedTime;

            var dateTime = new DateTime(dateTimeUtcNow.Year, dateTimeUtcNow.Month, dateTimeUtcNow.Day,
                doctorSchedule.BeginWork.Hours, doctorSchedule.BeginWork.Minutes, doctorSchedule.BeginWork.Seconds);

            var bookedTime = Next(dateTime, doctorSchedule.DayOfWeek);

            var result = await _iRecordCommand.UpdateBookedTimeAsync(record.RecordId, bookedTime);

            result.Should().NotBeNull();
            result.RecordId.Should().Be(record.RecordId);
            result.BookedTime.Should().NotBe(oldRecordbookedTime);
        }

        [Fact]
        public async Task DeleteAsync_ShouldBeExpected()
        {
            var records = _fixture.Build<Record>().CreateMany();

            await _hospitalContextFixture.InitRecordsAsync(records);

            var record = _hospitalContextFixture.context.Records.FirstOrDefault();

            await _iRecordCommand.DeleteAsync(record.RecordId);

            var result = _hospitalContextFixture.context.Records;

            result.Should().NotContain(record);
        }

        public DateTime Next(DateTime from, DayOfWeek dayOfTheWeek)
        {
            var date = from.Date.AddMonths(1);
            var days = (((int)dayOfTheWeek - (int)date.DayOfWeek + 7) % 7) + 7;
            return date.AddDays(days);
        }
    }
}
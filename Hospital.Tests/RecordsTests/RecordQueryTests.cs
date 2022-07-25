using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hospital.Application.CQRS.Records.Record.Queries;
using Hospital.Application.CQRS.Records.Views;
using Moq;
using Xunit;
using Record = Hospital.Domain.Record;

namespace Hospital.Tests.RecordsTests
{
    public class RecordQueryTests : IClassFixture<HospitalContextFixture>
    {
        private readonly IFixture _fixture = new Fixture();
        private readonly HospitalContextFixture _hospitalContextFixture;
        private readonly IRecordQuery _iRecordQuery;
        private readonly IMapper _mapper;

        public RecordQueryTests()
        {
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _fixture.Customizations.Add(new PersonExternalIdGenerator());

            _hospitalContextFixture = new Mock<HospitalContextFixture>().Object;

            var config = MapperForTests.GetConfiguration();
            _mapper = new Mapper(config);

            _iRecordQuery = new RecordQuery(_hospitalContextFixture.context, _mapper);
        }

        [Fact]
        public async Task GetAll_ShouldBeExpected()
        {
            var records = _fixture.Build<Record>().CreateMany();

            await _hospitalContextFixture.InitRecordsAsync(records);

            var result = _iRecordQuery.GetAll();

            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByDoctorId_ShouldBeExpected()
        {
            var records = _fixture.Build<Record>().CreateMany();

            await _hospitalContextFixture.InitRecordsAsync(records);

            var doctorId = records.Select(i => i.DoctorId).FirstOrDefault();

            var expectedCount = records.Where(i => i.DoctorId.Equals(doctorId));

            var result = _iRecordQuery.GetByDoctorId(doctorId);

            result.Should().NotBeNull();
            result.Count().Should().Be(expectedCount.Count());
        }

        [Fact]
        public async Task GetByUserId_ShouldBeExpected()
        {
            var records = _fixture.Build<Record>().CreateMany();

            await _hospitalContextFixture.InitRecordsAsync(records);

            var patientId = records.Select(i => i.PatientId).FirstOrDefault();

            var expectedCount = records.Where(i => i.PatientId.Equals(patientId));

            var result = _iRecordQuery.GetByUserId(patientId);

            result.Should().NotBeNull();
            result.Count().Should().Be(expectedCount.Count());
        }

        [Fact]
        public void GetByUserId_ShouldThrowsArgumentException()
        {
            var patientId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() => _iRecordQuery.GetByUserId(patientId));
        }

        [Fact]
        public void GetByDoctorId_ShouldThrowsArgumentException()
        {
            var doctorId = Guid.NewGuid();

            Assert.Throws<ArgumentException>(() => _iRecordQuery.GetByDoctorId(doctorId));
        }
    }
}
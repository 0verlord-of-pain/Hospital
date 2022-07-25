using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.Records.Record.Commands;
using Hospital.Application.CQRS.Records.Record.Queries;
using Hospital.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/users/records")]
    public class RecordController : Controller
    {
        private readonly IRecordCommand _iRecordCommand;
        private readonly IRecordQuery _iRecordQuery;

        public RecordController(
            IRecordCommand iRecordCommand,
            IRecordQuery iRecordQuery)
        {
            _iRecordCommand = iRecordCommand;
            _iRecordQuery = iRecordQuery;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var result = _iRecordQuery.GetAll();
            return Ok(result);
        }

        [HttpGet("doctor/{doctorId}")]
        [AllowAnonymous]
        public IActionResult GetByDoctorId([FromRoute] Guid doctorId)
        {
            var result = _iRecordQuery.GetByDoctorId(doctorId);
            return Ok(result);
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Policy = Policies.AdminOrManagerOrDoctor)]
        public IActionResult GetByUserId([FromRoute] Guid patientId)
        {
            var result = _iRecordQuery.GetByUserId(patientId);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> CreateAsync(
            [FromQuery] Guid doctorId,
            [FromQuery] Guid userId,
            [FromQuery] DateTime bookedTime)
        {
            var result = await _iRecordCommand.CreateAsync(userId, doctorId, bookedTime);
            return Ok(result);
        }

        [HttpPut("{recordId}")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] Guid recordId,
            [FromQuery] DateTime bookedTime)
        {
            var result = await _iRecordCommand.UpdateBookedTimeAsync(recordId, bookedTime);
            return Ok(result);
        }

        [HttpDelete("{recordId}")]
        [Authorize(Policy = Policies.AdminOrManager)]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] Guid recordId)
        {
            await _iRecordCommand.DeleteAsync(recordId);
            return NoContent();
        }
    }
}
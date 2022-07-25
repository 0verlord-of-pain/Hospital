using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.UserAccounts.Doctors.Commands;
using Hospital.Application.CQRS.UserAccounts.Doctors.Queries;
using Hospital.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/users/doctors")]
    public class DoctorController : Controller
    {
        private readonly IDoctorCommand _iDoctorCommand;
        private readonly IDoctorQuery _iDoctorQuery;

        public DoctorController(
            IDoctorQuery iDoctorQuery,
            IDoctorCommand iDoctorCommand)
        {
            _iDoctorQuery = iDoctorQuery;
            _iDoctorCommand = iDoctorCommand;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var result = _iDoctorQuery.GetDoctors();
            return Ok(result);
        }

        [HttpGet("{doctorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorByIdAsync([FromRoute] Guid doctorId)
        {
            var result = await _iDoctorQuery.GetDoctorByIdAsync(doctorId);
            return Ok(result);
        }

        [HttpPost("{userId}")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> CreateAsync(
            [FromRoute] Guid userId,
            [FromQuery] string profession)
        {
            var result = await _iDoctorCommand.CreateAsync(userId, profession);
            return Ok(result);
        }

        [HttpPut("{doctorId}")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> UpdateProfessionAsync(
            [FromRoute] Guid doctorId,
            [FromQuery] string profession)
        {
            var result = await _iDoctorCommand.UpdateProfessionAsync(doctorId, profession);
            return Ok(result);
        }

        [HttpDelete("{doctorId}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid doctorId)
        {
            await _iDoctorCommand.DeleteAsync(doctorId);
            return NoContent();
        }
    }
}
using System;
using System.Threading.Tasks;
using Hospital.Application.CQRS.Schedules.Schedule.Commands;
using Hospital.Application.CQRS.Schedules.Schedule.Queries;
using Hospital.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers
{
    [Route("api/users/doctors/shedules")]
    public class SheduleController : Controller
    {
        private readonly IScheduleCommand _iScheduleCommand;
        private readonly IScheduleQuery _iScheduleQuery;

        public SheduleController(
            IScheduleQuery iScheduleQuery,
            IScheduleCommand iScheduleCommand)
        {
            _iScheduleQuery = iScheduleQuery;
            _iScheduleCommand = iScheduleCommand;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var result = _iScheduleQuery.GetAll();
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> CreateAsync(
            [FromQuery] Guid doctorId,
            [FromQuery] DayOfWeek dayOfWeek,
            [FromQuery] TimeSpan beginWork,
            [FromQuery] TimeSpan endWork)
        {
            var result = await _iScheduleCommand.CreateAsync(doctorId, dayOfWeek, beginWork, endWork);
            return Ok(result);
        }

        [HttpPut("{scheduleId}")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] Guid scheduleId,
            [FromQuery] TimeSpan beginWork,
            [FromQuery] TimeSpan endWork)
        {
            var result = await _iScheduleCommand.UpdateAsync(scheduleId, beginWork, endWork);
            return Ok(result);
        }

        [HttpDelete("{scheduleId}")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute] Guid scheduleId)
        {
            await _iScheduleCommand.DeleteAsync(scheduleId);
            return NoContent();
        }
    }
}
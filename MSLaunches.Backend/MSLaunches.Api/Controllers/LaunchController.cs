using Microsoft.AspNetCore.Mvc;
using MSLaunches.Api.Filters;
using MSLaunches.Api.Models;
using MSLaunches.Data.Models;
using MSLaunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLaunches.Api.Controllers
{
    [Route("api/launches")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public class LaunchController : Controller
    {
        private readonly ILaunchService _launchService;

        public LaunchController(ILaunchService launchService)
        {
            _launchService = launchService;
        }

        /// <summary>
        /// Gets a list of launchs
        /// </summary>
        /// <response code="200">A list of launchs</response>
        /// <return>A list of launchs</return>
        [HttpGet]
        [ProducesResponseType(typeof(List<Launch>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _launchService.GetAsync());
        }

        /// <summary>
        /// Gets a launch based on his id
        /// </summary>
        /// <param name="id" cref="Guid">Guid of the launch</param>
        /// <response code="200">The launch that has the given id</response>
        /// <response code="404">Launch with the given id was not found</response>
        /// <return>A launchs</return>
        [HttpGet("{id}")]
        [ValidateModel]
        [ProducesResponseType(typeof(Launch), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var launch = await _launchService.GetByIdAsync(id);
            if (launch == null)
            {
                return NotFound();
            }

            return Ok(launch);
        }

        /// <summary>
        /// Creates a new launch
        /// </summary>
        /// <param name="launch" cref="LaunchDto">Launch model</param>
        /// <response code="204">Launch created</response>
        /// <response code="404">Launch could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]LaunchDto launch)
        {
            if (launch == null)
            {
                return BadRequest();
            }

            var affectedRows = await _launchService.CreateAsync(new Launch
            {
                Id = Guid.NewGuid(),
                LaunchName = launch.LaunchName,
                LaunchTypeId = launch.LaunchTypeId,
                CreatedOn = DateTime.Now,
                CreatedBy = "Test"
                // TODO: get createdBy from current launch
            });

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        ///<summary>
        /// Updates an launch given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the launch</param>
        ///<param name="launch" cref="LaunchDto">Launch model</param>
        ///<response code="204">Launch created</response>
        ///<response code="404">Launch not found / Launch could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]LaunchDto launch)
        {
            if (launch == null)
            {
                return BadRequest();
            }

            var affectedRows = await _launchService.UpdateAsync(new Launch
            {
                Id = id,
                LaunchName = launch.LaunchName,
                LaunchTypeId = launch.LaunchTypeId,
                CreatedOn = DateTime.Now,
                CreatedBy = "Test"
                // TODO: get UpdatedBy from current launch
            });

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        ///<summary>
        /// Deletes an launch given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the launch</param>
        ///<response code="204">Launch Deleted</response>
        ///<response code="404">Launch not found / Launch could not be deleted</response>
        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete(Guid id)
        {
            var affectedRows = await _launchService.DeleteByIdAsync(id);

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        /// <summary>
        /// A list of available launches by week
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<DailyLaunch>), 200)]
        public async Task<IActionResult> GetAllLaunchesAvailableByWeek()
        {
            return Ok(await _launchService.GetAllLaunchesAvailableByWeek());
        }

        /// <summary>
        /// Creates a new UserLaunch
        /// </summary>
        /// <param name="userLaunches" cref="UserLaunchDto">UserLaunch model</param>
        /// <response code="204">Launch created</response>
        /// <response code="404">Launch could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateUserLaunch([FromBody]List<UserLaunchDto> userLaunches)
        {
            if (userLaunches == null)
            {
                return BadRequest();
            }
            var userLaunchList = new List<UserLaunch>();
            foreach (var userLaunchDto in userLaunches)
            {
                var userLaunch = new UserLaunch
                {
                    Id = Guid.NewGuid(),
                    DailyLaunchId = userLaunchDto.DailyLaunchId,
                    UserId = userLaunchDto.UserId,
                    CreatedBy = "Test"
                    // TODO: get createdBy from current launch
                };
                userLaunchList.Add(userLaunch);
            }
            var affectedRows = await _launchService.CreateUserLaunchAsync(userLaunchList);
            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        /// <summary>
        /// Creates a new UserLaunch
        /// </summary>
        /// <param name="dailyLaunches" cref="DailyLaunchDto">DailyLaunch model</param>
        /// <response code="204">Launch created</response>
        /// <response code="404">Launch could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateDailyLaunch([FromBody]List<DailyLaunchDto> dailyLaunches)
        {
            if (dailyLaunches == null)
            {
                return BadRequest();
            }
            var dailyLaunchList = new List<DailyLaunch>();
            foreach (var dailyLaunchDto in dailyLaunches)
            {
                var dailyLaunch = new DailyLaunch
                {
                    Id = Guid.NewGuid(),
                    LaunchId = dailyLaunchDto.LaunchId,
                    Date = dailyLaunchDto.Date,
                    CreatedBy = "Test"
                    // TODO: get createdBy from current launch
                };
                dailyLaunchList.Add(dailyLaunch);
            }
            var affectedRows = await _launchService.CreateDailyLaunchesAsync(dailyLaunchList);
            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }
    }
}


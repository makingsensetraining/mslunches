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
                LaunchDescription = launch.LaunchDescription,
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
                LaunchDescription = launch.LaunchDescription,
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
    }
}


using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSDailyLunches.Api.Controllers
{
    [Route("api/dailyLunches")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public class UserDailyLunchController : Controller
    {
        private readonly IDailyLunchService _dailyLunchService;

        public UserDailyLunchController(IDailyLunchService dailyLunchService)
        {
            _dailyLunchService = dailyLunchService;
        }

        /// <summary>
        /// Gets a list of lunchs
        /// </summary>
        /// <response code="200">A list of lunchs</response>
        /// <return>A list of lunchs</return>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(List<DailyLunch>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _dailyLunchService.GetAsync());
        }

        /// <summary>
        /// Gets a lunch based on his id
        /// </summary>
        /// <param name="id" cref="Guid">Guid of the lunch</param>
        /// <response code="200">The lunch that has the given id</response>
        /// <response code="404">DailyLunch with the given id was not found</response>
        /// <return>A lunchs</return>
        [HttpGet("{id}")]
        [ValidateModel]
        [ProducesResponseType(typeof(DailyLunch), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var lunch = await _dailyLunchService.GetByIdAsync(id);
            if (lunch == null)
            {
                return NotFound();
            }

            return Ok(lunch);
        }

        /// <summary>
        /// Creates a new lunch
        /// </summary>
        /// <param name="lunch" cref="DailyLunchDto">DailyLunch model</param>
        /// <response code="204">DailyLunch created</response>
        /// <response code="404">DailyLunch could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]DailyLunchDto lunch)
        {
            if (lunch == null)
            {
                return BadRequest();
            }

            var affectedRows = await _dailyLunchService.CreateAsync(new DailyLunch
            {
                Id = Guid.NewGuid(),
                Date = lunch.Date,
                LunchId = lunch.LunchId,
                CreatedOn = DateTime.Now,
                CreatedBy = "Test"
                // TODO: get createdBy from current lunch
            });

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        ///<summary>
        /// Updates an lunch given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the lunch</param>
        ///<param name="lunch" cref="DailyLunchDto">DailyLunch model</param>
        ///<response code="204">DailyLunch created</response>
        ///<response code="404">DailyLunch not found / DailyLunch could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]DailyLunchDto lunch)
        {
            if (lunch == null)
            {
                return BadRequest();
            }

            var affectedRows = await _dailyLunchService.UpdateAsync(new DailyLunch
            {
                Id = id,
                Date = lunch.Date,
                LunchId = lunch.LunchId,
                CreatedOn = DateTime.Now,
                CreatedBy = "Test"
                // TODO: get UpdatedBy from current lunch
            });

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        ///<summary>
        /// Deletes an lunch given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the lunch</param>
        ///<response code="204">DailyLunch Deleted</response>
        ///<response code="404">DailyLunch not found / DailyLunch could not be deleted</response>
        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete(Guid id)
        {
            var affectedRows = await _dailyLunchService.DeleteByIdAsync(id);

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        /// <summary>
        /// A list of available lunches by week
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllLunchesAvailableInWeek")]
        [ProducesResponseType(typeof(List<DailyLunch>), 200)]
        public async Task<IActionResult> GetAllLunchesAvailableInWeek()
        {
            return Ok(await _dailyLunchService.GetAllLunchesAvailableInWeek());
        }

        
        /// <summary>
        /// Creates a new UserDailyLunch
        /// </summary>
        /// <param name="dailyDailyLunches" cref="DailyLunchDto">DailyLunch model</param>
        /// <response code="204">DailyLunch created</response>
        /// <response code="404">DailyLunch could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWeekLunch([FromBody]List<DailyLunchDto> dailyDailyLunches)
        {
            if (dailyDailyLunches == null)
            {
                return BadRequest();
            }
            var dailyDailyLunchList = new List<DailyLunch>();
            foreach (var dailyDailyLunchDto in dailyDailyLunches)
            {
                var dailyDailyLunch = new DailyLunch
                {
                    Id = Guid.NewGuid(),
                    LunchId = dailyDailyLunchDto.LunchId,
                    Date = dailyDailyLunchDto.Date,
                    CreatedBy = "Test"
                    // TODO: get createdBy from current lunch
                };
                dailyDailyLunchList.Add(dailyDailyLunch);
            }
            var affectedRows = await _dailyLunchService.CreateDailyLunchesAsync(dailyDailyLunchList);
            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }
    }
}
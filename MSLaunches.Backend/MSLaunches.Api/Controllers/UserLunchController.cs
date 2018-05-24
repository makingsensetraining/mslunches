using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace MSLunches.Api.Controllers
{
    [Route("api/users/{userId}/lunches")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public class UserLunchController : Controller
    {
        private readonly IUserLunchService _userLunchService;

        public UserLunchController(IUserLunchService userLunchService)
        {
            _userLunchService = userLunchService;
        }

        /// <summary>
        /// Gets a list of lanches selections by users
        /// </summary>
        /// <response code="200">A list of lunches selected by user</response>
        /// <return>A list of UserLunch</return>
        [HttpGet()]
        [ProducesResponseType(typeof(List<UserLunch>), 200)]
        public async Task<IActionResult> GetAll(string userId)
        {
            return Ok(await _userLunchService.GetAsync(userId));
        }

        /// <summary>
        /// Gets a meal selection by user based on his id
        /// </summary>
        /// <param name="id" cref="Guid">Guid of the UserLunch</param>
        /// <response code="200">The UserLunch that has the given id</response>
        /// <response code="404">UserLunch with the given id was not found</response>
        /// <return>A lunchs</return>
        [HttpGet("{id}")]
        [ValidateModel]
        [ProducesResponseType(typeof(UserLunch), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var lunch = await _userLunchService.GetByIdAsync(id);
            if (lunch == null)
            {
                return NotFound();
            }

            return Ok(lunch);
        }

        /// <summary>
        /// Creates a new meal selection for user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userLunch" cref="InputUserLunchDto">UserLunch model</param>
        /// <response code="204">UserLunch created</response>
        /// <response code="422">UserLunch could not be created - Already exists</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create(
            [FromRoute]string userId,
            [FromBody]InputUserLunchDto userLunch)
        {
            if (userLunch == null) return BadRequest();

            var userLunchToCreate = new UserLunch
            {
                LunchId = userLunch.LunchId,
                UserId = userId,
                Approved = userLunch.Approved,
                CreatedBy = "Test" //TODO: Add user.-
            };

            var existingUserLunch = _userLunchService.GetUserLunchByUserAndLunchIdAsync(userId, userLunch.LunchId);
            if (existingUserLunch != null) return StatusCode(422, "UserLumch already exists");

            var result = await _userLunchService.CreateAsync(userLunchToCreate);

            return CreatedAtAction(nameof(Get), new { userId = result.Id }, new UserLunchDto(result));
        }

        ///<summary>
        /// Updates a meal selection by user.
        ///</summary>
        /// <param name="userId"></param>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<param name="userLunch" cref="InputUserLunchDto">UserLunch model</param>
        ///<response code="204">UserLunch created</response>
        ///<response code="404">UserLunch not found / UserLunch could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(
            [FromRoute]string userId,
            [FromRoute]Guid id,
            [FromBody]InputUserLunchDto userLunch)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (userLunch == null) return BadRequest();

            var userLunchToUpdate = new UserLunch
            {
                Id = id,
                LunchId = userLunch.LunchId,
                UserId = userId,
                Approved = userLunch.Approved,
                UpdatedBy = "Test" //TODO: Add user.
            };

            var result = await _userLunchService.UpdateAsync(userLunchToUpdate);

            if (result == null) return NotFound();

            return NoContent();
        }

        ///<summary>
        /// Deletes an meal given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<response code="204">Meal Deleted</response>
        ///<response code="404">Meal not found / Meal could not be deleted</response>
        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete(Guid id)
        {
            var affectedRows = await _userLunchService.DeleteByIdAsync(id);

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        /// <summary>
        /// A list of available lunches by week
        /// </summary>
        /// <returns></returns>
        [HttpGet("LunchesByUserAndWeek")]
        [ProducesResponseType(typeof(List<UserLunch>), 200)]
        public async Task<IActionResult> GetlLunchesByUserByWeek(string userId)
        {
            return Ok(await _userLunchService.GetlLunchesByUserByWeekAsync(userId));
        }
    }
}
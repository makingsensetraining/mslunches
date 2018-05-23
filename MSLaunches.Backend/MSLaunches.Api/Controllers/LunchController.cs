﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;

namespace MSLunches.Api.Controllers
{
    [Route("api/lunches")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public class LunchController : Controller
    {
        private readonly ILunchService _lunchService;

        public LunchController(ILunchService lunchService)
        {
            _lunchService = lunchService;
        }

        /// <summary>
        /// Gets a list of lunchs
        /// </summary>
        /// <response code="200">A list of lunchs</response>
        /// <return>A list of lunchs</return>
        [HttpGet()]
        [ProducesResponseType(typeof(List<Lunch>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lunchService.GetAsync());
        }

        /// <summary>
        /// Gets a meal based on his id
        /// </summary>
        /// <param name="id" cref="Guid">Guid of the meal</param>
        /// <response code="200">The meal that has the given id</response>
        /// <response code="404">Lunch with the given id was not found</response>
        /// <return>A lunchs</return>
        [HttpGet("{id}")]
        [ValidateModel]
        [ProducesResponseType(typeof(Lunch), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var lunch = await _lunchService.GetByIdAsync(id);
            if (lunch == null)
            {
                return NotFound();
            }

            return Ok(lunch);
        }

        /// <summary>
        /// Creates a new Lunch
        /// </summary>
        /// <param name="lunch" cref="InputLunchDto">User data</param>
        /// <response code="201">User created</response>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(Lunch), 201)]
        public async Task<IActionResult> Create([FromBody]InputLunchDto lunch)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (lunch == null) return BadRequest();

            var lunchToCreate = new Lunch
            {
                Date = lunch.Date,
                MealId = lunch.MealId
            };

            var result = await _lunchService.CreateAsync(lunchToCreate);

            return CreatedAtAction(nameof(Get), new { userId = result.Id }, new LunchDto(result));
        }

        ///<summary>
        /// Updates an meal given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<param name="lunchDto" cref="InputLunchDto">Lunch model</param>
        ///<response code="204">Lunch created</response>
        ///<response code="404">Lunch not found / Lunch could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]InputLunchDto lunchDto)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (lunchDto == null) return BadRequest();

            var lunchToUpdate = new Lunch
            {
                Id = id,
                Date = lunchDto.Date,
                MealId = lunchDto.MealId,
                UpdatedBy = "Test" //TODO: Add user.
            };

            var result = await _lunchService.UpdateAsync(lunchToUpdate);

            if (result == null) return NotFound();

            return NoContent();
        }

        ///<summary>
        /// Deletes an meal given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<response code="204">Lunch Deleted</response>
        ///<response code="404">Lunch not found / Lunch could not be deleted</response>
        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete(Guid id)
        {
            var affectedRows = await _lunchService.DeleteByIdAsync(id);

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

        /// <summary>
        /// A list of available lunches by week
        /// </summary>
        /// <returns></returns>
        [HttpGet("LunchesAvailables")]
        [ProducesResponseType(typeof(List<Lunch>), 200)]
        public async Task<IActionResult> LunchesAvailables()
        {
            return Ok(await _lunchService.GetAllLunchesAvailableInWeek());
        }


        /// <summary>
        /// Creates a new UserLunch
        /// </summary>
        /// <param name="lunches" cref="LunchDto">Lunch model</param>
        /// <response code="204">Lunch created</response>
        /// <response code="404">Lunch could not be created</response>
        [HttpPost("LunchSelection")]
        [ValidateModel]
        public async Task<IActionResult> LunchSelection([FromBody]List<LunchDto> lunches)
        {
            if (lunches == null)
            {
                return BadRequest();
            }
            var lunchList = new List<Lunch>();
            foreach (var lunchDto in lunches)
            {
                var lunch = new Lunch
                {
                    Id = Guid.NewGuid(),
                    MealId = lunchDto.MealId,
                    Date = lunchDto.Date,
                    CreatedBy = "Test"
                    // TODO: get createdBy from current meal
                };
                lunchList.Add(lunch);
            }
            var affectedRows = await _lunchService.CreateLunchesAsync(lunchList);
            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }
    }
}
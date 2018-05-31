using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models.Request;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSLunches.Api.Controllers
{
    [Route("api/meals")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        /// <summary>
        /// Gets a list of Meals
        /// </summary>
        /// <response code="200">A list of meals</response>
        /// <return>A list of meals</return>
        [HttpGet]
        [ProducesResponseType(typeof(List<MealResponse>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok((await _mealService.GetAsync())
                .Select(meal => new MealResponse(meal)));
        }

        /// <summary>
        /// Gets a meal based on his id
        /// </summary>
        /// <param name="id" cref="Guid">Guid of the meal</param>
        /// <response code="200">The meal that has the given id</response>
        /// <response code="404">Meal with the given id was not found</response>
        /// <return>A meals</return>
        [HttpGet("{id}")]
        [ValidateModel]
        [ProducesResponseType(typeof(MealResponse), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var meal = await _mealService.GetByIdAsync(id);
            if (meal == null)
            {
                return NotFound();
            }

            return Ok(new MealResponse(meal));
        }

        /// <summary>
        /// Creates a new meal
        /// </summary>
        /// <param name="meal" cref="MealRequest">Meal model</param>
        /// <response code="204">Meal created</response>
        /// <response code="404">Meal could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]MealRequest meal)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (meal == null) return BadRequest();

            var mealToCreate = new Meal
            {
                Name = meal.Name,
                TypeId = meal.TypeId
            };

            var result = await _mealService.CreateAsync(mealToCreate);

            return CreatedAtAction(
                nameof(Get), 
                new { id = result.Id }, 
                new MealResponse(result));
        }

        ///<summary>
        /// Updates an meal given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<param name="meal" cref="MealRequest">Meal model</param>
        ///<response code="204">Meal created</response>
        ///<response code="404">Meal not found / Meal could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]MealRequest meal)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (meal == null) return BadRequest();

            var mealToUpdate = new Meal
            {
                Id = id,
                Name = meal.Name,
                TypeId = meal.TypeId,
                UpdatedBy = "Test" //TODO: Add user.
            };

            var result = await _mealService.UpdateAsync(mealToUpdate);

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
            var affectedRows = await _mealService.DeleteByIdAsync(id);

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }

    }
}
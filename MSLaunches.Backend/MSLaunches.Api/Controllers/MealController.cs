using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models.Request;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Api.Controllers
{
    [Route("api/meals")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly IMapper _mapper;

        public MealController(
            IMealService mealService,
            IMapper mapper)
        {
            _mealService = mealService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of Meals
        /// </summary>
        /// <response code="200">A list of meals</response>
        /// <return>A list of meals</return>
        [HttpGet]
        [ProducesResponseType(typeof(List<MealDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<MealDto>>(await _mealService.GetAsync()));
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
        [ProducesResponseType(typeof(MealDto), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var meal = await _mealService.GetByIdAsync(id);
            if (meal == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MealDto>(meal));
        }

        /// <summary>
        /// Creates a new meal
        /// </summary>
        /// <param name="meal" cref="InputMealDto">Meal model</param>
        /// <response code="204">Meal created</response>
        /// <response code="404">Meal could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]InputMealDto meal)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (meal == null) return BadRequest();

            var result = await _mealService.CreateAsync(
                _mapper.Map<Meal>(meal));

            return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                _mapper.Map<MealDto>(result));
        }

        ///<summary>
        /// Updates an meal given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<param name="meal" cref="InputMealDto">Meal model</param>
        ///<response code="204">Meal created</response>
        ///<response code="404">Meal not found / Meal could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]InputMealDto meal)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (meal == null) return BadRequest();

            var result = await _mealService.UpdateAsync(
                _mapper.Map<Meal>(meal));

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
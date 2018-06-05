using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Api.Controllers
{
    [Route("api/mealtypes")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorDto), 500)]
    public class MealTypeController : Controller
    {
        private readonly IMealTypeService _mealTypeService;

        public MealTypeController(IMealTypeService mealTypeService)
        {
            _mealTypeService = mealTypeService;
        }

        /// <summary>
        /// Gets a list of MealTypeTypes
        /// </summary>
        /// <response code="200">A list of mealTypes</response>
        /// <return>A list of mealTypes</return>
        [HttpGet]
        [ProducesResponseType(typeof(List<MealType>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mealTypeService.GetAsync());
        }

        /// <summary>
        /// Gets a mealType based on his id
        /// </summary>
        /// <param name="id" cref="int">identifier of the mealType</param>
        /// <response code="200">The mealType that has the given id</response>
        /// <response code="404">MealType with the given id was not found</response>
        /// <return>A mealTypes</return>
        [HttpGet("{id}")]
        [ValidateModel]
        [ProducesResponseType(typeof(MealType), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var mealType = await _mealTypeService.GetByIdAsync(id);
            if (mealType == null)
            {
                return NotFound();
            }

            return Ok(mealType);
        }
    }
}
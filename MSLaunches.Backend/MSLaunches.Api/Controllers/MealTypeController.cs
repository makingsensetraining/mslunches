using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models.Response;
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
        private readonly IMapper _mapper;

        public MealTypeController(
            IMealTypeService mealTypeService,
            IMapper mapper)
        {
            _mealTypeService = mealTypeService;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets a list of MealTypeTypes
        /// </summary>
        /// <response code="200">A list of mealTypes</response>
        /// <return>A list of mealTypes</return>
        [HttpGet]
        [ProducesResponseType(typeof(List<MealTypeDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<MealTypeDto>>(
                await _mealTypeService.GetAsync()));
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
        [ProducesResponseType(typeof(MealTypeDto), 200)]
        public async Task<IActionResult> Get(int id)
        {
            var mealType = await _mealTypeService.GetByIdAsync(id);
            if (mealType == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MealTypeDto>(mealType));
        }
    }
}
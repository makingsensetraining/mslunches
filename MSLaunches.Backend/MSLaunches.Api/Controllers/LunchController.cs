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
    [Route("api/lunches")]
    [Produces("Application/json")]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public class LunchController : Controller
    {
        #region Fields

        private readonly ILunchService _lunchService;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public LunchController(
            ILunchService lunchService,
            IMapper mapper)
        {
            _lunchService = lunchService;
            _mapper = mapper;
        }

        #endregion

        #region Query Endpoints

        /// <summary>
        /// Gets a list of lunches
        /// </summary>
        /// <response code="200">A list of lunches</response>
        /// <return>A list of lunchs</return>
        [HttpGet]
        [ProducesResponseType(typeof(List<LunchDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<LunchDto>>(
                await _lunchService.GetAsync()));
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
        [ProducesResponseType(typeof(LunchDto), 200)]
        public async Task<IActionResult> Get(Guid id)
        {
            var lunch = await _lunchService.GetByIdAsync(id);
            if (lunch == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LunchDto>(lunch));
        }

        #endregion

        #region Command Endpoints

        /// <summary>
        /// Creates a new Lunch
        /// </summary>
        /// <param name="lunch" cref="InputLunchDto">User data</param>
        /// <response code="201">User created</response>
        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(LunchDto), 201)]
        public async Task<IActionResult> Create([FromBody]InputLunchDto lunch)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (lunch == null) return BadRequest();

            var result = await _lunchService.CreateAsync(
                _mapper.Map<Lunch>(lunch));

            return CreatedAtAction(
                nameof(Get),
                new { id = result.Id },
                _mapper.Map<LunchDto>(result));
        }

        [HttpPost]
        [ValidateModel]
        [ProducesResponseType(typeof(List<LunchDto>), 200)]
        public async Task<IActionResult> BatchSave([FromBody]List<InputLunchDto> lunches)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (lunches == null) return BadRequest();

            var result = await _lunchService.CreateLunchesAsync(
                _mapper.Map<List<Lunch>>(lunches));

            return Ok(_mapper.Map<List<LunchDto>>(result));
        }

        ///<summary>
        /// Updates an meal given his id
        ///</summary>
        ///<param name="id" cref="Guid">Guid of the meal</param>
        ///<param name="lunchDto" cref="LunchDto">Lunch model</param>
        ///<response code="204">Lunch created</response>
        ///<response code="404">Lunch not found / Lunch could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]InputLunchDto lunchDto)
        {
            // TODO: Fix validation attribute, it's not working as expected.
            if (lunchDto == null) return BadRequest();

            var lunch = _mapper.Map<Lunch>(lunchDto);
            lunch.Id = id;

            var result = await _lunchService.UpdateAsync(lunch);

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

        #endregion
    }
}
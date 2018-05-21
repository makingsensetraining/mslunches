using Microsoft.AspNetCore.Mvc;
using MSLunches.Api.Filters;
using MSLunches.Api.Models;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        [HttpGet]
        [ProducesResponseType(typeof(List<Lunch>), 200)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _lunchService.GetAsync());
        }

        /// <summary>
        /// Gets a lunch based on his id
        /// </summary>
        /// <param name="id" cref="Guid">Guid of the lunch</param>
        /// <response code="200">The lunch that has the given id</response>
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
        /// Creates a new lunch
        /// </summary>
        /// <param name="lunch" cref="LunchDto">Lunch model</param>
        /// <response code="204">Lunch created</response>
        /// <response code="404">Lunch could not be created</response>
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]LunchDto lunch)
        {
            if (lunch == null)
            {
                return BadRequest();
            }

            var affectedRows = await _lunchService.CreateAsync(new Lunch
            {
                Id = Guid.NewGuid(),
                LunchName = lunch.LunchName,
                LunchTypeId = lunch.LunchTypeId,
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
        ///<param name="lunch" cref="LunchDto">Lunch model</param>
        ///<response code="204">Lunch created</response>
        ///<response code="404">Lunch not found / Lunch could not be updated</response>
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(Guid id, [FromBody]LunchDto lunch)
        {
            if (lunch == null)
            {
                return BadRequest();
            }

            var affectedRows = await _lunchService.UpdateAsync(new Lunch
            {
                Id = id,
                LunchName = lunch.LunchName,
                LunchTypeId = lunch.LunchTypeId,
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
        ///<response code="204">Lunch Deleted</response>
        ///<response code="404">Lunch not found / Lunch could not be deleted</response>
        [HttpDelete("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Delete(Guid id)
        {
            var affectedRows = await _lunchService.DeleteByIdAsync(id);

            return affectedRows == 0 ? NotFound() : NoContent() as IActionResult;
        }
        
    }
}
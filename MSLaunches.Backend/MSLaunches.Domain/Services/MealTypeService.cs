using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services
{
    public class MealTypeService : IMealTypeService
    {
        #region Members

        private readonly WebApiCoreLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealTypeService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WebApiCoreLunchesContext"/> instance required to access database </param>
        public MealTypeService(WebApiCoreLunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        public async Task<MealType> GetByIdAsync(Guid mealTypeId)
        {
            return await _dbContext.MealTypes
                                   .FindAsync(mealTypeId);
        }

        public async Task<List<MealType>> GetAsync()
        {
            return await _dbContext.MealTypes
                                   .Include(x => x.Meals)
                                   .ToListAsync();
        }


        #endregion

    }
}

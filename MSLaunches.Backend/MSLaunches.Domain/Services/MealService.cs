using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services
{
    public class MealService : IMealService
    {
        #region Members

        private readonly WebApiCoreLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WebApiCoreLunchesContext"/> instance required to access database </param>
        public MealService(WebApiCoreLunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        public async Task<Meal> GetByIdAsync(Guid mealId)
        {
            return await _dbContext.Meals
                                   .FindAsync(mealId);
        }

        public async Task<List<Meal>> GetAsync()
        {
            return await _dbContext.Meals
                                   .Include(a => a.MealType)
                                   .ToListAsync();
        }

        public async Task<Meal> CreateAsync(Meal meal)
        {
            meal.CreatedOn = DateTime.Now;
            _dbContext.Meals
                      .Add(meal);
            await _dbContext.SaveChangesAsync();
            return meal;
        }

        public async Task<Meal> UpdateAsync(Meal meal)
        {

            var mealToUpdate = await _dbContext.Meals
                                                .FindAsync(meal.Id);

            if (mealToUpdate == null) return null;

            mealToUpdate.Name = meal.Name;
            mealToUpdate.TypeId = meal.TypeId;
            mealToUpdate.UpdatedBy = meal.UpdatedBy;
            mealToUpdate.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return mealToUpdate;
        }

        public async Task<int> DeleteByIdAsync(Guid mealId)
        {
            var meal = await _dbContext.Meals
                                        .FindAsync(mealId);
            if (meal == null)
            {
                return 0;
            }

            _dbContext.Meals
                      .Remove(meal);
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

    }
}

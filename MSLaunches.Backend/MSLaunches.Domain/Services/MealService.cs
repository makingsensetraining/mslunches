using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services
{
    /// <inheritdoc />
    public class MealService : IMealService
    {
        #region Members

        private readonly MSLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MealService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="MSLunchesContext"/> instance required to access database </param>
        public MealService(MSLunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Query Methods

        /// <inheritdoc />
        public async Task<Meal> GetByIdAsync(Guid mealId)
        {
            return await _dbContext.Meals
                                   .FindAsync(mealId);
        }

        /// <inheritdoc />
        public async Task<List<Meal>> GetAsync()
        {
            return await _dbContext.Meals
                                   .Include(a => a.MealType)
                                   .ToListAsync();
        }

        #endregion

        #region Public Command Methods

        /// <inheritdoc />
        public async Task<Meal> CreateAsync(Meal meal)
        {
            // Fill needed data
            meal.Id = Guid.NewGuid();
            meal.CreatedOn = DateTime.Now;

            // Add and save
            var result = _dbContext.Meals.Add(meal);
            await _dbContext.SaveChangesAsync();

            return result.Entity;
        }

        /// <inheritdoc />
        public async Task<Meal> UpdateAsync(Meal meal)
        {
            // Find Meal
            var mealToUpdate = 
                await _dbContext.Meals.FindAsync(meal.Id);

            if (mealToUpdate == null) return null;

            // Update data
            mealToUpdate.Name = meal.Name;
            mealToUpdate.TypeId = meal.TypeId;
            mealToUpdate.UpdatedBy = meal.UpdatedBy;
            mealToUpdate.UpdatedOn = DateTime.Now;

            // Save Changes
            await _dbContext.SaveChangesAsync();
            return mealToUpdate;
        }

        /// <inheritdoc />
        public async Task<int> DeleteByIdAsync(Guid mealId)
        {
            // Find meal
            var meal = 
                await _dbContext.Meals.FindAsync(mealId);

            if (meal == null) return 0;

            // Remove and save changes
            _dbContext.Meals.Remove(meal);
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

    }
}

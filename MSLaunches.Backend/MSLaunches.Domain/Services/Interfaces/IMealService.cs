using MSLunches.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services.Interfaces
{
    /// <summary>
    /// Service responsible of handling meals
    /// </summary>
    public interface IMealService
    {
        /// <summary>
        /// Gets all the existing meals
        /// </summary>
        /// <returns>List with all the existing meals</returns>
        Task<List<Meal>> GetAsync();

        /// <summary>
        /// Gets a meal by Id
        /// </summary>
        /// <param name="mealId">Id of the meal to be retrieved</param>
        /// <returns>A <see cref="Meal"/> object if the meal is found, otherwise null</returns>
        Task<Meal> GetByIdAsync(Guid mealId);

        /// <summary>
        /// Creates a meal
        /// </summary>
        /// <param name="meal">Meal to create</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<Meal> CreateAsync(Meal meal);

        /// <summary>
        /// Updates a meal
        /// </summary>
        /// <param name="meal">Meal to update</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<Meal> UpdateAsync(Meal meal);

        /// <summary>
        /// Deletes a meal by Id
        /// </summary>
        /// <param name="mealId">Id of the meal to delete</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> DeleteByIdAsync(Guid mealId);
    }
}
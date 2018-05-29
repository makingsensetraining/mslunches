using MSLunches.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services.Interfaces
{
    /// <summary>
    /// Service responsible of handling mealTypes
    /// </summary>
    public interface IMealTypeService
    {
        /// <summary>
        /// Gets a mealType by Id
        /// </summary>
        /// <param name="mealTypeId">Id of the mealType to be retrieved</param>
        /// <returns>A <see cref="MealType"/> object if the mealType is found, otherwise null</returns>
        Task<MealType> GetByIdAsync(Guid mealTypeId);

        /// <summary>
        /// Gets all the existing mealTypes
        /// </summary>
        /// <returns>List with all the existing mealTypes</returns>
        Task<List<MealType>> GetAsync();
    }
}
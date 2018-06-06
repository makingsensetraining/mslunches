using MSLunches.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services.Interfaces
{
    /// <summary>
    /// Service responsible of handling lunchs
    /// </summary>
    public interface IUserLunchService
    {
        /// <summary>
        /// Gets all the existing lunchs, 
        /// if setted star date and end date returns only a subset
        /// </summary>
        /// <param name="userId"> owner of the lunches </param>
        /// <param name="startDate"> start date filter </param>
        /// <param name="endDate">end date filter </param>
        /// <returns>List with all the existing lunchs</returns>
        Task<List<UserLunch>> GetAsync(string userId, DateTime? startDate = null, DateTime? endDate = null);

        /// <summary>
        /// Gets a userLunch by Id
        /// </summary>
        /// <param name="userLunchId">Id of the userLunch to be retrieved</param>
        /// <returns>A <see cref="Lunch"/> object if the userLunch is found, otherwise null</returns>
        Task<UserLunch> GetByIdAsync(Guid userLunchId);

        /// <summary>
        /// Creates a userLunch
        /// </summary>
        /// <param name="userLunch">Meal to create</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<UserLunch> CreateAsync(UserLunch userLunch);

        /// <summary>
        /// Deletes a userLunch by Id
        /// </summary>
        /// <param name="lunchId">Id of the userLunch to delete</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> DeleteByIdAsync(Guid lunchId);

        /// <summary>
        /// Updates a userLunch
        /// </summary>
        /// <param name="userLunch">updated userLunch</param>
        /// <exception cref="NotFoundException">When UserLunch or Lunch does not exist</exception>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<UserLunch> UpdateAsync(UserLunch userLunch);
    }
}
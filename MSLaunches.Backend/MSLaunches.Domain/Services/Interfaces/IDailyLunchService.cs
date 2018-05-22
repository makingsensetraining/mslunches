using MSLunches.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services.Interfaces
{
    /// <summary>
    /// Service responsible of handling lunchs
    /// </summary>
    public interface IDailyLunchService
    {
        /// <summary>
        /// Gets a dailyLunch by Id
        /// </summary>
        /// <param name="dailyLunchId">Id of the dailyLunch to be retrieved</param>
        /// <returns>A <see cref="DailyLunch"/> object if the lunch is found, otherwise null</returns>
        Task<DailyLunch> GetByIdAsync(Guid dailyLunchId);

        /// <summary>
        /// Creates a dailyLunch
        /// </summary>
        /// <param name="dailyLunch">DailyLunch to create</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<DailyLunch> CreateAsync(DailyLunch dailyLunch);

        Task<int> CreateDailyLunchesAsync(List<DailyLunch> dailyLunches);

        /// <summary>
        /// Deletes a dailyLunch by Id
        /// </summary>
        /// <param name="lunchId">Id of the dailyLunch to delete</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> DeleteByIdAsync(Guid lunchId);

        /// <summary>
        /// Gets all the existing lunchs
        /// </summary>
        /// <returns>List with all the existing lunchs</returns>
        Task<List<DailyLunch>> GetAsync();

        /// <summary>
        /// Updates a dailyLunch
        /// </summary>
        /// <param name="dailyLunch">dailyLunch to update</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<DailyLunch> UpdateAsync(DailyLunch dailyLunch);

        /// <summary>
        ///A list of available lunches by week
        /// </summary>
        /// <returns></returns>
        Task<List<DailyLunch>> GetAllLunchesAvailableInWeek();
    }
}
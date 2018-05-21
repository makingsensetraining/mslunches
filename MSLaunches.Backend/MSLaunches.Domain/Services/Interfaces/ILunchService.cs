using MSLunches.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services.Interfaces
{
    /// <summary>
    /// Service responsible of handling lunchs
    /// </summary>
    public interface ILunchService
    {
        /// <summary>
        /// Gets a lunch by Id
        /// </summary>
        /// <param name="lunchId">Id of the lunch to be retrieved</param>
        /// <returns>A <see cref="Lunch"/> object if the lunch is found, otherwise null</returns>
        Task<Lunch> GetByIdAsync(Guid lunchId);

        /// <summary>
        /// Creates a lunch
        /// </summary>
        /// <param name="lunch">Lunch to create</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> CreateAsync(Lunch lunch);

        /// <summary>
        /// Deletes a lunch by Id
        /// </summary>
        /// <param name="lunchId">Id of the lunch to delete</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> DeleteByIdAsync(Guid lunchId);

        /// <summary>
        /// Gets all the existing lunchs
        /// </summary>
        /// <returns>List with all the existing lunchs</returns>
        Task<List<Lunch>> GetAsync();

        /// <summary>
        /// Updates a lunch
        /// </summary>
        /// <param name="lunch">Lunch to update</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> UpdateAsync(Lunch lunch);
    }
}
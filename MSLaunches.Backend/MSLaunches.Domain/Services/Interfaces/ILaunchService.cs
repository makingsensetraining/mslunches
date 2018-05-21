using MSLaunches.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSLaunches.Domain.Services.Interfaces
{
    /// <summary>
    /// Service responsible of handling launchs
    /// </summary>
    public interface ILaunchService
    {
        /// <summary>
        /// Gets a launch by Id
        /// </summary>
        /// <param name="launchId">Id of the launch to be retrieved</param>
        /// <returns>A <see cref="Launch"/> object if the launch is found, otherwise null</returns>
        Task<Launch> GetByIdAsync(Guid launchId);

        /// <summary>
        /// Creates a launch
        /// </summary>
        /// <param name="launch">Launch to create</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> CreateAsync(Launch launch);

        /// <summary>
        /// Deletes a launch by Id
        /// </summary>
        /// <param name="launchId">Id of the launch to delete</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> DeleteByIdAsync(Guid launchId);

        /// <summary>
        /// Gets all the existing launchs
        /// </summary>
        /// <returns>List with all the existing launchs</returns>
        Task<List<Launch>> GetAsync();

        /// <summary>
        /// Updates a launch
        /// </summary>
        /// <param name="launch">Launch to update</param>
        /// <returns>An integer indicating the amount of affected rows</returns>
        Task<int> UpdateAsync(Launch launch);
        
        /// <summary>
        ///A list of available launches by week
        /// </summary>
        /// <returns></returns>
        Task<List<DailyLaunch>> GetAllLaunchesAvailableByWeek();

    }
}
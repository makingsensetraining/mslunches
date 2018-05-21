﻿using Microsoft.EntityFrameworkCore;
using MSLaunches.Data.EF;
using MSLaunches.Data.Models;
using MSLaunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSLaunches.Domain.Services
{
    public class LaunchService : ILaunchService
    {
        private readonly WebApiCoreLaunchesContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LaunchService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WebApiCoreLaunchesContext"/> instance required to access database </param>
        public LaunchService(WebApiCoreLaunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Launch> GetByIdAsync(Guid launchId)
        {
            return await _dbContext.Launches.FindAsync(launchId);
        }

        public async Task<List<Launch>> GetAsync()
        {
            return await _dbContext.Launches.ToListAsync();
        }

        public async Task<int> CreateAsync(Launch launch)
        {
            launch.CreatedOn = DateTime.Now;
            _dbContext.Launches.Add(launch);
            return await _dbContext.SaveChangesAsync();
        }


        public async Task<int> CreateUserLaunchAsync(List<UserLaunch> userLaunches)
        {
            foreach (var userLaunch in userLaunches)
            {
                userLaunch.CreatedOn = DateTime.Now;
                _dbContext.UserLaunch.Add(userLaunch);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateDailyLaunchesAsync(List<DailyLaunch> dailyLaunches)
        {
            foreach (var dailyLaunch in dailyLaunches)
            {
                dailyLaunch.CreatedOn = DateTime.Now;
                _dbContext.DailyLaunch.Add(dailyLaunch);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Launch launch)
        {
            var launchToUpdate = await _dbContext.Launches.FirstOrDefaultAsync(u => u.Id == launch.Id);

            if (launchToUpdate == null)
            {
                return 0;
            }

            launchToUpdate.LaunchName = launch.LaunchName;
            launchToUpdate.LaunchType = launch.LaunchType;
            launchToUpdate.UpdatedBy = launch.UpdatedBy;
            launchToUpdate.UpdatedOn = DateTime.Now;

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteByIdAsync(Guid launchId)
        {
            var launch = await _dbContext.Launches.FirstOrDefaultAsync(item => item.Id == launchId);
            if (launch == null)
            {
                return 0;
            }

            _dbContext.Launches.Remove(launch);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<DailyLaunch>> GetAllLaunchesAvailableByWeek()
        {
            var daysInWeek = GetDaysInWeek();
            return await (from x in _dbContext.DailyLaunch
                          where daysInWeek.Contains(x.Date)
                          select x).ToListAsync();
        }

        private List<DateTime> GetDaysInWeek()
        {
            var now = DateTime.Now;
            var currentDay = now.DayOfWeek;
            int days = (int)currentDay;
            DateTime sunday = now.AddDays(-days);
            return Enumerable.Range(0, 7)
                .Select(d => sunday.AddDays(d))
                .ToList();
        }
    }
}
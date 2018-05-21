using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDailyLunches.Domain.Services
{
    public class DailyLunchService : IDailyLunchService
    {
        #region Members

        private readonly WebApiCoreLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DailyLunchService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WebApiCoreDailyLunchesContext"/> instance required to access database </param>
        public DailyLunchService(WebApiCoreLunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        public async Task<DailyLunch> GetByIdAsync(Guid dailyLunchId)
        {
            return await _dbContext.DailyLunches
                                   .FindAsync(dailyLunchId);
        }

        public async Task<List<DailyLunch>> GetAsync()
        {
            return await _dbContext.DailyLunches.ToListAsync();
        }

        public async Task<int> CreateAsync(DailyLunch dailyLunch)
        {
            dailyLunch.CreatedOn = DateTime.Now;
            _dbContext.DailyLunches
                      .Add(dailyLunch);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateDailyLunchesAsync(List<DailyLunch> dailyDailyLunches)
        {
            foreach (var dailyDailyLunch in dailyDailyLunches)
            {
                dailyDailyLunch.CreatedOn = DateTime.Now;
                _dbContext.DailyLunches
                          .Add(dailyDailyLunch);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(DailyLunch dailyLunch)
        {
            var dailyLunchToUpdate = await _dbContext.DailyLunches
                                                 .FirstOrDefaultAsync(u => u.Id == dailyLunch.Id);

            if (dailyLunchToUpdate == null)
            {
                return 0;
            }

            dailyLunchToUpdate.Date = dailyLunch.Date;
            dailyLunchToUpdate.LunchId = dailyLunch.LunchId;
            dailyLunchToUpdate.UpdatedBy = dailyLunch.UpdatedBy;
            dailyLunchToUpdate.UpdatedOn = DateTime.Now;

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteByIdAsync(Guid dailyLunchId)
        {
            var dailyLunch = await _dbContext.DailyLunches
                                              .FirstOrDefaultAsync(item => item.Id == dailyLunchId);
            if (dailyLunch == null)
            {
                return 0;
            }

            _dbContext.DailyLunches
                      .Remove(dailyLunch);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<DailyLunch>> GetAllLunchesAvailableInWeek()
        {
            var daysInWeek = GetDaysInWeek();
            return await _dbContext.DailyLunches
                                   .Where(x => daysInWeek.Contains(x.Date))
                                   .ToListAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the days in the week of that belongs today
        /// </summary>
        /// <returns></returns>
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

        #endregion
    }
}

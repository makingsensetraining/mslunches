using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSLunches.Domain.Services
{
    public class LunchService : ILunchService
    {
        #region Members

        private readonly WebApiCoreLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LunchService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WebApiCoreLunchesContext"/> instance required to access database </param>
        public LunchService(WebApiCoreLunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        public async Task<Lunch> GetByIdAsync(Guid lunchId)
        {
            return await _dbContext.Lunches
                                   .FindAsync(lunchId);
        }

        public async Task<List<Lunch>> GetAsync()
        {
            return await _dbContext.Lunches.ToListAsync();
        }

        public async Task<Lunch> CreateAsync(Lunch lunch)
        {
            lunch.CreatedOn = DateTime.Now;
            _dbContext.Lunches
                      .Add(lunch);
            await _dbContext.SaveChangesAsync();
            return lunch;
        }

        public async Task<int> CreateLunchesAsync(List<Lunch> lunches)
        {
            foreach (var lunch in lunches)
            {
                lunch.CreatedOn = DateTime.Now;
                _dbContext.Lunches
                          .Add(lunch);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Lunch> UpdateAsync(Lunch lunch)
        {
            var lunchToUpdate = await _dbContext.Lunches
                                                     .FirstOrDefaultAsync(u => u.Id == lunch.Id);

            if (lunchToUpdate == null) return null;

            lunchToUpdate.Date = lunch.Date;
            lunchToUpdate.MealId = lunch.MealId;
            lunchToUpdate.UpdatedBy = lunch.UpdatedBy;
            lunchToUpdate.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return lunchToUpdate;
        }

        public async Task<int> DeleteByIdAsync(Guid lunchId)
        {
            var lunch = await _dbContext.Lunches
                                              .FirstOrDefaultAsync(item => item.Id == lunchId);
            if (lunch == null)
            {
                return 0;
            }

            _dbContext.Lunches
                      .Remove(lunch);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Lunch>> GetAllLunchesAvailableInWeek()
        {
            var daysInWeek = GetDaysInWeek();
            return await _dbContext.Lunches
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

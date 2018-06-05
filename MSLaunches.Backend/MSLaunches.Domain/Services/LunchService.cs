using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Exceptions;
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

        private readonly MSLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LunchService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="MSLunchesContext"/> instance required to access database </param>
        public LunchService(MSLunchesContext dbContext)
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
            var contex = _dbContext.Lunches
                .Include(a => a.Meal)
                    .ThenInclude(a => a.Type);
            return await contex.ToListAsync();
        }

        public async Task<Lunch> CreateAsync(Lunch lunch)
        {
            await AddLunch(lunch);
            await _dbContext.SaveChangesAsync();
            return lunch;
        }

        /// <summary>
        /// Delete the existing lunches in the week, to create the new ones.-
        /// </summary>
        /// <param name="lunches"></param>
        /// <returns></returns>
        public async Task<List<Lunch>> BatchSaveAsync(List<Lunch> lunches)
        {
            var dateFrom = lunches.Min(x => x.Date).Date;
            var dateTo = lunches.Max(x => x.Date).Date;

            var lunchesToUpdate = lunches.Where(a => a.Id != Guid.Empty);
            var lunchesToCreate = lunches.Where(a => a.Id == Guid.Empty);

            foreach (var lunchToUpdate in lunchesToUpdate)
            {
                await ChangeLunch(lunchToUpdate);
            }

            foreach (var lunchToCreate in lunchesToCreate)
            {
                await AddLunch(lunchToCreate);
            }

            await _dbContext.SaveChangesAsync();
            return await _dbContext.Lunches.Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToListAsync();
        }

        public async Task<Lunch> UpdateAsync(Lunch lunch)
        {
            var res = await ChangeLunch(lunch);
            await _dbContext.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteByIdAsync(Guid lunchId)
        {
            var lunch = await _dbContext.Lunches.FindAsync(lunchId);
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

        public async Task<List<Lunch>> GetLunchesBetweenDatesAsync(DateTime dateFrom, DateTime dateTo)
        {
            var contex = _dbContext.Lunches
                .Where(x => x.Date >= dateFrom && x.Date <= dateTo)
                .Include(a => a.Meal)
                .ThenInclude(a => a.Type);
            return await contex.ToListAsync();
        }

        #endregion

        #region Private Methods

        private async Task<Lunch> ChangeLunch(Lunch lunch)
        {
            var lunchToUpdate = await _dbContext.Lunches.FindAsync(lunch.Id);

            if (lunchToUpdate == null)
                throw new NotFoundException($"Lunch with id {lunch.Id} was not found");

            lunchToUpdate.Date = lunch.Date.Date;
            lunchToUpdate.MealId = lunch.MealId;
            lunchToUpdate.UpdatedBy = lunch.UpdatedBy;
            lunchToUpdate.UpdatedOn = DateTime.Now;
            return lunchToUpdate;
        }

        private async Task AddLunch(Lunch lunch)
        {
            var meal = await _dbContext.Meals.FindAsync(lunch.MealId);
            if (meal == null)
            {
                throw new ValidationException($"meal with id {lunch.MealId}, does not exist");
            }

            if (await _dbContext.Lunches.AnyAsync(a => a.Date == lunch.Date.Date && a.Meal.TypeId == meal.TypeId))
            {
                throw new ValidationException($"lunch with the same meal and date already exist");
            }

            lunch.CreatedOn = DateTime.Now;
            lunch.Date = lunch.Date.Date;
            await _dbContext.Lunches.AddAsync(lunch);
        }

        /// <summary>
        /// Get the days in the week of that belongs today
        /// </summary>
        /// <returns></returns>
        private List<DateTime> GetDaysInWeek()
        {
            var now = DateTime.Now;
            var currentDay = now.DayOfWeek;
            var days = (int)currentDay;
            var sunday = now.AddDays(-days);
            return Enumerable.Range(0, 7)
                             .Select(d => sunday.AddDays(d))
                             .ToList();
        }

        #endregion
    }
}

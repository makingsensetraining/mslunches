using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<Lunch> UpdateAsync(Lunch lunch)
        {

            var lunchToUpdate = await _dbContext.Lunches
                                                .FindAsync(lunch.Id);

            if (lunchToUpdate == null) return null;

            lunchToUpdate.LunchName = lunch.LunchName;
            lunchToUpdate.LunchTypeId = lunch.LunchTypeId;
            lunchToUpdate.UpdatedBy = lunch.UpdatedBy;
            lunchToUpdate.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return lunchToUpdate;
        }

        public async Task<int> DeleteByIdAsync(Guid lunchId)
        {
            var lunch = await _dbContext.Lunches
                                        .FindAsync(lunchId);
            if (lunch == null)
            {
                return 0;
            }

            _dbContext.Lunches
                      .Remove(lunch);
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

    }
}

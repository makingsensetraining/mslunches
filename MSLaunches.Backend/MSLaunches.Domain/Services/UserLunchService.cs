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
    public class UserLunchService : IUserLunchService
    {
        #region Members

        private readonly WebApiCoreLunchesContext _dbContext;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLunchService"/> class.
        /// </summary>
        /// <param name="dbContext"><see cref="WebApiCoreLunchesContext"/> instance required to access database </param>
        public UserLunchService(WebApiCoreLunchesContext dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

        #region Public Methods

        public async Task<UserLunch> GetByIdAsync(Guid userLunchId)
        {
            return await _dbContext.UserLunches
                                   .FindAsync(userLunchId);
        }

        public async Task<List<UserLunch>> GetAsync()
        {
            return await _dbContext.UserLunches
                                   .ToListAsync();
        }

        public async Task<UserLunch> CreateAsync(UserLunch userLunch)
        {
            userLunch.Id = Guid.NewGuid();
            userLunch.CreatedOn = DateTime.Now;

            await _dbContext.UserLunches.AddAsync(userLunch);
            await _dbContext.SaveChangesAsync();

            return userLunch;
        }

        public async Task<UserLunch> UpdateAsync(UserLunch userLunch)
        {
            var userLunchToUpdate = await _dbContext.UserLunches.FindAsync(userLunch.Id);

            if (userLunchToUpdate == null) return null;
           
            userLunchToUpdate.DailyLunchId = userLunch.DailyLunchId;
            userLunchToUpdate.UserId = userLunch.UserId;
            userLunchToUpdate.Approved = userLunch.Approved;
            userLunchToUpdate.UpdatedBy = userLunch.UpdatedBy;
            userLunchToUpdate.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return userLunchToUpdate;
        }

        public async Task<int> CreateUserLunchesAsync(List<UserLunch> userLunches)
        {
            foreach (var userLunch in userLunches)
            {
                userLunch.CreatedOn = DateTime.Now;
                _dbContext.UserLunches
                          .Add(userLunch);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteByIdAsync(Guid userLunchId)
        {
            var userLunch = await _dbContext.UserLunches
                                         .FirstOrDefaultAsync(item => item.Id == userLunchId);
            if (userLunch == null)
            {
                return 0;
            }

            _dbContext.UserLunches
                      .Remove(userLunch);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<List<UserLunch>> GetlLunchesByUserByWeekAsync(Guid userId)
        {
            return await _dbContext.UserLunches
                                   .Where(x => x.UserId == userId)
                                   .ToListAsync();
        }

        #endregion

    }
}

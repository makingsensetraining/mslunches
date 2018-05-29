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

        #region Public query methods
        /// <inheritdoc />
        public async Task<UserLunch> GetByIdAsync(Guid userLunchId)
        {
            return await _dbContext.UserLunches
                                   .FindAsync(userLunchId);
        }

        /// <inheritdoc />
        public async Task<List<UserLunch>> GetAsync(string userId)
        {
            return await _dbContext.UserLunches
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        #endregion

        #region Public command methods

        /// <inheritdoc />
        public async Task<UserLunch> CreateAsync(UserLunch userLunch)
        {
            //Pre processing
            userLunch.Id = Guid.NewGuid();
            userLunch.CreatedOn = DateTime.Now;

            //Pre Conditions
            var lastLunch = await _dbContext.UserLunches.FirstOrDefaultAsync(a => a.LunchId == userLunch.LunchId && a.UserId == userLunch.UserId);
            if (lastLunch != null)
                throw new ValidationException("User Lunch already exists");

            var requestedLunch = await _dbContext.Lunches.FirstOrDefaultAsync(a => a.Id == userLunch.LunchId);
            if (requestedLunch == null)
                throw new NotFoundException($"Lunch with id {userLunch.LunchId} does not exist");

            ValidateExpiration(requestedLunch);

            //Actual Action
            await _dbContext.UserLunches.AddAsync(userLunch);
            await _dbContext.SaveChangesAsync();

            return userLunch;
        }

        /// <inheritdoc />
        public async Task<UserLunch> UpdateAsync(UserLunch userLunch)
        {
            //Pre Condition
            var userLunchToUpdate = await _dbContext.UserLunches.FindAsync(userLunch.Id);
            if (userLunchToUpdate == null)
                throw new NotFoundException($"User Lunch with id {userLunch.Id} does not exist");

            var requestedLunch = await _dbContext.Lunches.FirstOrDefaultAsync(a => a.Id == userLunch.LunchId);
            if (requestedLunch == null)
                throw new NotFoundException($"Lunch with id {userLunch.LunchId} does not exist");

            ValidateExpiration(requestedLunch);

            //Actual Action
            userLunchToUpdate.LunchId = userLunch.LunchId;
            userLunchToUpdate.UserId = userLunch.UserId;
            userLunchToUpdate.Approved = userLunch.Approved;
            userLunchToUpdate.UpdatedBy = userLunch.UpdatedBy;
            userLunchToUpdate.UpdatedOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return userLunchToUpdate;
        }

        /// <inheritdoc />
        public async Task<int> DeleteByIdAsync(Guid userLunchId)
        {
            var userLunch = await _dbContext.UserLunches.FindAsync(userLunchId);
            if (userLunch == null)
                throw new NotFoundException($"User Lunch with id {userLunch.Id} does not exist");

            _dbContext.UserLunches.Remove(userLunch);
            return await _dbContext.SaveChangesAsync();
        }

        #endregion

        #region Private Methods

        private static void ValidateExpiration(Lunch requestedLunch)
        {
            if (requestedLunch.Date < DateTime.Today.AddHours(10))
                throw new ValidationException($"Lunch with id {requestedLunch.Id} has expired");
        }

        #endregion

    }
}

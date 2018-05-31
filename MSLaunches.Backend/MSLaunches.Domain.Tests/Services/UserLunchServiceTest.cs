using Microsoft.EntityFrameworkCore;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MSLunches.Domain.Tests.Services
{
    public class UserLunchServiceTest
    {
        #region Get Tests

        [Fact]
        public async Task GetAsync_ReturnsUserLunches()
        {
            var userId = "09123091823";
            var userLunch1 = GetUserLunch(Guid.NewGuid(), Guid.NewGuid(), userId);
            var userLunch2 = GetUserLunch(Guid.NewGuid(), Guid.NewGuid(), userId);
            var userLunch3 = GetUserLunch();

            var dbOptions = GetDbOptions("GetAsync_ReturnsLunches");

            using (var context = new MSLunchesContext(dbOptions))
            {
                await context.UserLunches.AddAsync(userLunch1);
                await context.UserLunches.AddAsync(userLunch2);
                await context.UserLunches.AddAsync(userLunch3);
                await context.SaveChangesAsync();
            }

            IEnumerable<UserLunch> result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.GetAsync(userId);
            }

            Assert.NotNull(result);
            Assert.Contains(result, lunch => Equals(lunch, userLunch1));
            Assert.Contains(result, lunch => Equals(lunch, userLunch2));
            Assert.DoesNotContain(result, lunch => Equals(lunch, userLunch3));
        }

        #endregion

        #region GetById tests

        [Fact]
        public async Task GetByIdAsync_ReturnsLunch_WhenIdExist()
        {
            var userLunch = GetUserLunch(Guid.NewGuid());

            var dbOptions = GetDbOptions("GetByIdAsync_ReturnsLunch_WhenIdExist");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.UserLunches.Add(userLunch);
                context.SaveChanges();
            }

            UserLunch result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.GetByIdAsync(userLunch.Id);
            }

            Assert.NotNull(result);
            Assert.True(Equals(userLunch, result));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenIdNotExist()
        {
            var dbOptions = GetDbOptions("GetByIdAsync_ReturnsLunch_WhenIdExist");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.UserLunches.Add(GetUserLunch());
                context.SaveChanges();
            }

            UserLunch result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.GetByIdAsync(Guid.NewGuid());
            }

            Assert.Null(result);
        }

        #endregion

        #region CreateAsync Tests

        [Fact]
        public async void CreateAsync_ReturnsCreatedLunch()
        {
            var lunch = GetUserLunch();
            UserLunch result = null;
            using (var context = new MSLunchesContext(GetDbOptions("CreateAsync_ReturnsCreatedLunch")))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.CreateAsync(lunch);
            }

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.NotEqual(default(DateTime), result.CreatedOn);
            Assert.Equal(lunch.Approved, result.Approved);
            Assert.Equal(lunch.LunchId, result.LunchId);
            Assert.Equal(lunch.UserId, result.UserId);
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async void UpdateAsync_ReturnsLunchModified_WhenIdExist()
        {
            var userLunch = GetUserLunch();
            var userLunchModified = new UserLunch
            {
                Id = userLunch.Id,
                Approved = !userLunch.Approved,
                LunchId = Guid.NewGuid(),
                UserId = "newUserId1",
                UpdatedBy = "updater"
            };

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsLunchModified_WhenIdExist")))
            {
                await context.UserLunches.AddAsync(userLunch);
                await context.SaveChangesAsync();
            }

            UserLunch result = null;

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsLunchModified_WhenIdExist")))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.UpdateAsync(userLunchModified);
            }

            Assert.NotNull(result);
            Assert.Equal(userLunchModified.Id, result.Id);
            Assert.NotEqual(userLunchModified.UpdatedOn, result.UpdatedOn);
            Assert.Equal(userLunchModified.UpdatedBy, result.UpdatedBy);
            Assert.Equal(userLunchModified.Approved, result.Approved);
            Assert.Equal(userLunchModified.UserId, result.UserId);
            Assert.Equal(userLunchModified.LunchId, result.LunchId);
        }

        [Fact]
        public async void UpdateAsync_ReturnsNull_WhenIdNotExist()
        {
            var lunchModified = GetUserLunch();

            UserLunch result = null;

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsNull_WhenIdNotExist")))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.UpdateAsync(lunchModified);
            }

            Assert.Null(result);
        }

        #endregion

        #region DeleteByIdAsync Tests

        [Fact]
        public async Task DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist()
        {
            var userLunch = GetUserLunch();

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                await context.UserLunches.AddAsync(userLunch);
                await context.SaveChangesAsync();
            }

            var result = 0;

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.DeleteByIdAsync(userLunch.Id);
            }

            Assert.True(result > 0);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsZero_WhenIdNotExist()
        {
            var result = 0;

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                var classUnderTest = new UserLunchService(context);
                result = await classUnderTest.DeleteByIdAsync(Guid.NewGuid());
            }

            Assert.True(result == 0);
        }

        #endregion

        #region Private Methods

        private DbContextOptions<MSLunchesContext> GetDbOptions(string context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MSLunchesContext>();
            optionsBuilder.UseInMemoryDatabase("GetByIdAsync_ShouldReturnUser");
            return optionsBuilder.Options;
        }

        private UserLunch GetUserLunch(Guid? id = null, Guid? lunchId = null, string userId = "")
        {
            return new UserLunch
            {
                Approved = true,
                Id = id ?? Guid.NewGuid(),
                LunchId = lunchId ?? Guid.NewGuid(),
                UserId = string.IsNullOrEmpty(userId) ? "someUser" : userId,
            };
        }

        private bool Equals(UserLunch userlunch1, UserLunch userLunch2)
        {
            return userlunch1.LunchId == userLunch2.LunchId
                && userlunch1.UserId == userLunch2.UserId
                && userlunch1.Id == userLunch2.Id;
        }

        #endregion
    }
}

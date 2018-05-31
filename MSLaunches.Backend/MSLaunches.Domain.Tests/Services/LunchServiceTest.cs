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
    public class LunchServiceTests
    {
        #region Get Tests

        [Fact]
        public async Task GetAsync_ReturnsLunches()
        {
            var mealType = new MealType
            {
                Id = 1,
                Description = "desc",
                IsSelectable = true
            };

            var meal = GetADefaultMeal(Guid.NewGuid(), mealType.Id);

            var lunch1 = GetADefaultLunch(Guid.NewGuid(), meal.Id);
            var lunch2 = GetADefaultLunch(Guid.NewGuid(), meal.Id);

            var dbOptions = GetDbOptions("GetAsync_ReturnsLunches");

            using (var context = new MSLunchesContext(dbOptions))
            {
                await context.MealTypes.AddAsync(mealType);
                await context.Meals.AddAsync(meal);
                await context.Lunches.AddAsync(lunch1);
                await context.Lunches.AddAsync(lunch2);
                await context.SaveChangesAsync();
            }

            IEnumerable<Lunch> result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.GetAsync();
            }

            Assert.NotNull(result);
            Assert.Contains(result, lunch => Equals(lunch, lunch1));
            Assert.Contains(result, lunch => Equals(lunch, lunch2));
        }

        #endregion

        #region GetById tests

        [Fact]
        public async Task GetByIdAsync_ReturnsLunch_WhenIdExist()
        {
            var lunch = GetADefaultLunch(Guid.NewGuid());

            var dbOptions = GetDbOptions("GetByIdAsync_ReturnsLunch_WhenIdExist");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.Lunches.Add(lunch);
                context.SaveChanges();
            }

            Lunch result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.GetByIdAsync(lunch.Id);
            }

            Assert.NotNull(result);
            Assert.True(Equals(lunch, result));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenIdNotExist()
        {
            var dbOptions = GetDbOptions("GetByIdAsync_ReturnsLunch_WhenIdExist");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.Lunches.Add(GetADefaultLunch());
                context.SaveChanges();
            }

            Lunch result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.GetByIdAsync(Guid.NewGuid());
            }

            Assert.Null(result);
        }

        #endregion

        #region CreateAsync Tests

        [Fact]
        public async void CreateAsync_ReturnsCreatedLunch()
        {
            var lunch = GetADefaultLunch();
            Lunch result = null;
            using (var context = new MSLunchesContext(GetDbOptions("CreateAsync_ReturnsCreatedLunch")))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.CreateAsync(lunch);
            }

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.NotEqual(default(DateTime), result.CreatedOn);
            Assert.Equal(lunch.MealId, result.MealId);
            Assert.Equal(lunch.Date, result.Date);
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async void UpdateAsync_ReturnsLunchModified_WhenIdExist()
        {
            var lunch = GetADefaultLunch();
            var lunchModified = new Lunch
            {
                Id = lunch.Id,
                Date = DateTime.Today.AddDays(-1),
                MealId = Guid.NewGuid(),
                UpdatedBy = "updater"
            };

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsLunchModified_WhenIdExist")))
            {
                await context.Lunches.AddAsync(lunch);
                await context.SaveChangesAsync();
            }

            Lunch result = null;

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsLunchModified_WhenIdExist")))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.UpdateAsync(lunchModified);
            }

            Assert.NotNull(result);
            Assert.Equal(lunchModified.Id, result.Id);
            Assert.NotEqual(lunchModified.UpdatedOn, result.UpdatedOn);
            Assert.Equal(lunchModified.UpdatedBy, result.UpdatedBy);
            Assert.Equal(lunchModified.MealId, result.MealId);
            Assert.Equal(lunchModified.Date, result.Date);
        }

        [Fact]
        public async void UpdateAsync_ReturnsNull_WhenIdNotExist()
        {
            var lunchModified = GetADefaultLunch();

            Lunch result = null;

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsNull_WhenIdNotExist")))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.UpdateAsync(lunchModified);
            }

            Assert.Null(result);
        }

        #endregion

        #region DeleteByIdAsync Tests

        [Fact]
        public async Task DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist()
        {
            var lunch = GetADefaultLunch();

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                await context.Lunches.AddAsync(lunch);
                await context.SaveChangesAsync();
            }

            var result = 0;

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                var classUnderTest = new LunchService(context);
                result = await classUnderTest.DeleteByIdAsync(lunch.Id);
            }

            Assert.True(result > 0);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsZero_WhenIdNotExist()
        {
            var result = 0;

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                var classUnderTest = new LunchService(context);
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

        private Lunch GetADefaultLunch(Guid? id = null, Guid? mealId = null)
        {
            return new Lunch
            {
                Id = id ?? Guid.NewGuid(),
                MealId = mealId ?? Guid.NewGuid(),
                Date = DateTime.Now
            };
        }

        private bool Equals(Lunch lunch1, Lunch lunch2)
        {
            return lunch1.Id == lunch2.Id
                && lunch1.MealId == lunch2.MealId
                && lunch1.Date == lunch2.Date;
        }

        private Meal GetADefaultMeal(Guid? id = null, int typeId = 0)
        {
            return new Meal
            {
                Id = id ?? Guid.NewGuid(),
                TypeId = typeId,
                Name = "Test"
            };
        }

        #endregion
    }
}

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
    public class MealServiceTests
    {
        #region Get Tests

        [Fact]
        public async Task GetAsync_ReturnsMeals()
        {
            var meal1 = GetADefaultMeal(Guid.NewGuid());
            var meal2 = GetADefaultMeal(Guid.NewGuid());

            var dbOptions = GetDbOptions("GetAsync_ReturnsMeals");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.MealTypes.Add(GetSampleMealType());
                context.Meals.Add(meal1);
                context.Meals.Add(meal2);
                context.SaveChanges();
            }

            IEnumerable<Meal> result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.GetAsync();
            }

            Assert.NotNull(result);
            Assert.Contains(result, meal => Equals(meal, meal1));
            Assert.Contains(result, meal => Equals(meal, meal2));
        }

        #endregion

        #region GetById tests

        [Fact]
        public async Task GetByIdAsync_ReturnsMeal_WhenIdExist()
        {
            var meal = GetADefaultMeal(Guid.NewGuid());

            var dbOptions = GetDbOptions("GetByIdAsync_ReturnsMeal_WhenIdExist");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.Meals.Add(meal);
                context.SaveChanges();
            }

            Meal result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.GetByIdAsync(meal.Id);
            }

            Assert.NotNull(result);
            Assert.True(Equals(meal, result));            
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenIdNotExist()
        {
            var dbOptions = GetDbOptions("GetByIdAsync_ReturnsMeal_WhenIdExist");

            using (var context = new MSLunchesContext(dbOptions))
            {
                context.Meals.Add(GetADefaultMeal());
                context.SaveChanges();
            }

            Meal result = null;

            using (var context = new MSLunchesContext(dbOptions))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.GetByIdAsync(Guid.NewGuid());
            }

            Assert.Null(result);
        }

        #endregion

        #region CreateAsync Tests

        [Fact]
        public async void CreateAsync_ReturnsCreatedMeal()
        {
            var meal = GetADefaultMeal();
            Meal result = null;
            using (var context = new MSLunchesContext(GetDbOptions("CreateAsync_ReturnsCreatedMeal")))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.CreateAsync(meal);
            }

            Assert.NotNull(result);
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.NotEqual(default(DateTime), result.CreatedOn);
            Assert.Equal(meal.TypeId, result.TypeId);
            Assert.Equal(meal.Name, result.Name);
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async void UpdateAsync_ReturnsMealModified_WhenIdExist()
        {
            var meal = GetADefaultMeal();
            var mealModified = new Meal
            {
                Id = meal.Id,
                Name = "newName",
                TypeId = meal.TypeId + 1,
                UpdatedBy = "updater"
            };

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsMealModified_WhenIdExist")))
            {
                await context.Meals.AddAsync(meal);
                await context.SaveChangesAsync();
            }

            Meal result = null;

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsMealModified_WhenIdExist")))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.UpdateAsync(mealModified);
            }

            Assert.NotNull(result);
            Assert.Equal(mealModified.Id, result.Id);
            Assert.NotEqual(mealModified.UpdatedOn, result.UpdatedOn);
            Assert.Equal(mealModified.UpdatedBy, result.UpdatedBy);
            Assert.Equal(mealModified.TypeId, result.TypeId);
            Assert.Equal(mealModified.Name, result.Name);
        }

        [Fact]
        public async void UpdateAsync_ReturnsNull_WhenIdNotExist()
        {
            var mealModified = GetADefaultMeal();

            Meal result = null;

            using (var context = new MSLunchesContext(GetDbOptions("UpdateAsync_ReturnsNull_WhenIdNotExist")))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.UpdateAsync(mealModified);
            }

            Assert.Null(result);
        }

        #endregion

        #region DeleteByIdAsync Tests

        [Fact]
        public async Task DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist()
        {
            var meal = GetADefaultMeal();

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                await context.Meals.AddAsync(meal);
                await context.SaveChangesAsync();
            }

            var result = 0;

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.DeleteByIdAsync(meal.Id);
            }

            Assert.True(result > 0);
        }

        [Fact]
        public async Task DeleteByIdAsync_ReturnsZero_WhenIdNotExist()
        {
            var result = 0;

            using (var context = new MSLunchesContext(GetDbOptions("DeleteByIdAsync_ReturnsCountOfChanges_WhenIdExist")))
            {
                var classUnderTest = new MealService(context);
                result = await classUnderTest.DeleteByIdAsync(Guid.NewGuid());
            }

            Assert.True(result == 0);
        }

        #endregion

        #region Private Methods

        private DbContextOptions<MSLunchesContext> GetDbOptions(string context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MSLunchesContext>();
            optionsBuilder.UseInMemoryDatabase(context);
            return optionsBuilder.Options;
        }

        private Meal GetADefaultMeal(Guid? id = null)
        {
            return new Meal
            {
                Id = id ?? Guid.NewGuid(),
                TypeId = 1,
                Name = "Test"
            };
        }

        private MealType GetSampleMealType(int id = 1)
        {
            return new MealType
            {
                Description = "somedesc",
                Id = id,
                IsSelectable = true
            };
        }

        private bool Equals(Meal meal1, Meal meal2) {
            return meal1.Id == meal2.Id
                && meal1.Name == meal2.Name
                && meal1.TypeId == meal2.TypeId;
        }

        #endregion
    }
}

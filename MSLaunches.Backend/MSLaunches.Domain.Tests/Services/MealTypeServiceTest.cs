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
    public class MealTypeServiceTest
    {
        #region Get Tests

        [Fact]
        public async Task Get_ReturnsAListOfMealTypes()
        {
            var mealType1 = GetSampleMealType(1);
            var mealType2 = GetSampleMealType(2);

            using (var context = new MSLunchesContext(GetDbOptions("Get_ReturnsAListOfMealTypes")))
            {
                context.MealTypes.Add(mealType1);
                context.MealTypes.Add(mealType2);
                context.Meals.Add(GetSampleMeal(1));
                context.SaveChanges();
            }

            IEnumerable<MealType> result;

            using (var context = new MSLunchesContext(GetDbOptions("Get_ReturnsAListOfMealTypes")))
            {
                var classUnderTest = new MealTypeService(context);
                result = await classUnderTest.GetAsync();
            }

            Assert.Contains(result, a => a.Id == mealType1.Id);
            Assert.Contains(result, a => a.Id == mealType2.Id);
        }

        #endregion

        #region GetById Tests

        public async Task GetById_RetunsAMealType()
        {
            var mealType = GetSampleMealType(1);

            using (var context = new MSLunchesContext(GetDbOptions("GetById_RetunsAMealType")))
            {
                context.MealTypes.Add(mealType);
                context.SaveChanges();
            }

            MealType result;

            using (var context = new MSLunchesContext(GetDbOptions("GetById_RetunsAMealType")))
            {
                var classUnderTest = new MealTypeService(context);
                result = await classUnderTest.GetByIdAsync(mealType.Id);
            }

            Assert.Equal(mealType.Id, result.Id);
        }

        #endregion

        #region Private methods

        private DbContextOptions<MSLunchesContext> GetDbOptions(string context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MSLunchesContext>();
            optionsBuilder.UseInMemoryDatabase(context);
            return optionsBuilder.Options;
        }

        private MealType GetSampleMealType(int id = 1)
        {
            return new MealType
            {
                Description = "desc1",
                Id = id,
                IsSelectable = true
            };
        }


        private Meal GetSampleMeal(int typeId = 1)
        {
            return new Meal
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.Now,
                CreatedBy = "creator",
                Name = "name",
                TypeId = typeId,
            };
        }

        #endregion
    }
}

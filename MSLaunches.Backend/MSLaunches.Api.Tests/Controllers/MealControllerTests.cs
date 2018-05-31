using Microsoft.AspNetCore.Mvc;
using Moq;
using MSLunches.Api.Controllers;
using MSLunches.Api.Models;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MSLunches.Api.Tests.Controllers
{
    public class MealControllerTests
    {
        private readonly Mock<IMealService> _mealService;

        public MealControllerTests()
        {
            _mealService = new Mock<IMealService>();
        }

        #region GetAll Test

        [Fact]
        public async void GetAll_ReturnsOk()
        {
            // Arrange
            var controller = new MealController(_mealService.Object);
            var sampleMeals = new List<Meal>()
            {
                GetSampleMeal(),
                GetSampleMeal(),
                GetSampleMeal()
            };

            _mealService.Setup(mock => mock.GetAsync()).ReturnsAsync(sampleMeals);

            // Act
            var result = await controller.GetAll();

            // Assert
            _mealService.Verify(mock => mock.GetAsync(), Times.Once);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            var meals = okObjectResult.Value as List<Meal>;

            Assert.Equal(sampleMeals.Count, meals.Count);
            foreach (var meal in meals)
            {
                var expected = sampleMeals.SingleOrDefault(u => u.Id == meal.Id);
                Assert.Equal(expected.Name, meal.Name);
                Assert.Equal(expected.TypeId, meal.TypeId);
            }
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_ReturnsOk()
        {
            // Arrange
            var controller = new MealController(_mealService.Object);
            var sampleMeal = GetSampleMeal();
            _mealService.Setup(mock => mock.GetByIdAsync(sampleMeal.Id)).ReturnsAsync(sampleMeal);

            // Act
            var result = await controller.Get(sampleMeal.Id);

            // Assert
            _mealService.Verify(mock => mock.GetByIdAsync(sampleMeal.Id), Times.Once);

            Assert.IsType<OkObjectResult>(result);
            var okObjectResult = result as OkObjectResult;
            var meal = okObjectResult.Value as Meal;

            Assert.Equal(sampleMeal.Name, meal.Name);
            Assert.Equal(sampleMeal.TypeId, meal.TypeId);
        }

        [Fact]
        public async void Get_ReturnsNotFound_WhenMealNotExists()
        {
            // Arrange
            var controller = new MealController(_mealService.Object);
            var mealId = Guid.NewGuid();
            _mealService.Setup(mock => mock.GetByIdAsync(mealId)).ReturnsAsync((Meal)null);

            // Act
            var result = await controller.Get(mealId);

            // Assert
            _mealService.Verify(mock => mock.GetByIdAsync(mealId), Times.Once);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Create test

        [Fact]
        public async void Create_ReturnsCreated()
        {
            // Arrange
            var controller = new MealController(_mealService.Object);
            var expected = GetSampleMeal();
            var sampleMeal = new InputMealDto
            {
                Name = expected.Name,
                TypeId = expected.TypeId
            };

            _mealService.Setup(mock => mock.CreateAsync(It.IsAny<Meal>())).ReturnsAsync(expected);

            // Act
            var result = await controller.Create(sampleMeal);

            // Assert
            _mealService.Verify(mock => mock.CreateAsync(It.IsAny<Meal>()), Times.Once);

            Assert.IsType<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            var meal = createdResult.Value as MealDto;

            Assert.Equal(expected.Id, meal.Id);
            Assert.Equal(expected.Name, meal.Name);
            Assert.Equal(expected.TypeId, meal.TypeId);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenMealIsNull()
        {
            // Arrange
            var classUnderTest = new MealController(_mealService.Object);

            // Act
            var res = await classUnderTest.Create(null);

            // Assert
            Assert.IsType<BadRequestResult>(res);
        }

        #endregion

        #region Update Test

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            // Arrange
            var controller = new MealController(_mealService.Object);
            var expected = GetSampleMeal();
            var sampleMeal = new InputMealDto
            {
                Name = expected.Name,
                TypeId = expected.TypeId
            };

            _mealService.Setup(mock => mock.UpdateAsync(It.IsAny<Meal>())).ReturnsAsync(expected);

            // Act
            var result = await controller.Update(expected.Id, sampleMeal);

            // Assert
            _mealService.Verify(mock => mock.UpdateAsync(It.IsAny<Meal>()), Times.Once);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadrequest_WhenMealIsNull()
        {
            // Arrange
            var classUnderTest = new MealController(_mealService.Object);

            // Act
            var res = await classUnderTest.Update(Guid.NewGuid(), null);

            // Assert
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenMealNotExists()
        {
            // Arrange
            var controller = new MealController(_mealService.Object);
            var sampleMeal = GetSampleInputMealDto();
            _mealService.Setup(mock => mock.UpdateAsync(It.IsAny<Meal>())).ReturnsAsync((Meal)null);

            // Act
            var result = await controller.Update(Guid.NewGuid(), sampleMeal);

            // Assert
            _mealService.Verify(mock => mock.UpdateAsync(It.IsAny<Meal>()), Times.Once);
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async void Delete_WhenIdExists_ShouldReturnNoContent()
        {
            var mealService = new Mock<IMealService>();
            var classUnderTest = new MealController(mealService.Object);

            var id = Guid.NewGuid();

            mealService.Setup(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(1);

            var result = await classUnderTest.Delete(id);

            Assert.IsType<NoContentResult>(result);
            mealService.VerifyAll();
        }

        [Fact]
        public async void Delete_WhenIdNotExists_ShouldReturnNotFound()
        {
            var mealService = new Mock<IMealService>();
            var classUnderTest = new MealController(mealService.Object);

            var id = Guid.NewGuid();

            mealService.Setup(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(0);

            var result = await classUnderTest.Delete(id);

            Assert.IsType<NotFoundResult>(result);
            mealService.VerifyAll();
        }

        #endregion

        #region Private methods

        private Meal GetSampleMeal(Guid? id = null)
        {
            return new Meal
            {
                Id = id ?? Guid.NewGuid(),
                Name = "Milanesas",
                TypeId = 2,
                CreatedOn = DateTime.Now
            };
        }

        private InputMealDto GetSampleInputMealDto()
        {
            return new InputMealDto
            {
                Name = "Papas fritas",
                TypeId = 1
            };
        }

        #endregion
    }
}

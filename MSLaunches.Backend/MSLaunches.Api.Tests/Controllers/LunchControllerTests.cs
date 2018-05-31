using Microsoft.AspNetCore.Mvc;
using Moq;
using MSLunches.Api.Controllers;
using MSLunches.Api.Models;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MSLunches.Api.Tests.Controllers
{
    public class LunchControllerTests
    {
        private Mock<ILunchService> _lunchService;

        public LunchControllerTests()
        {
            _lunchService = new Mock<ILunchService>();
        }

        #region GetAll Tests

        [Fact]
        public async Task GetAll_ReturnsLunches()
        {
            // Arrange
            var classUnderTest = new LunchController(_lunchService.Object);

            var listOfLunches = new List<Lunch>()
            {
                GetSampleLunch(),
                GetSampleLunch(),
                GetSampleLunch()
            };

            _lunchService.Setup(a => a.GetAsync()).ReturnsAsync(listOfLunches);

            // Act
            var result = await classUnderTest.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var resultList = Assert.IsType<List<Lunch>>(okResult.Value);

            foreach (var lunch in listOfLunches)
            {
                Assert.Contains(resultList, resultLunch => Equals(resultLunch, lunch));
            }

            _lunchService.Verify(a => a.GetAsync(), Times.Once);
        }

        #endregion

        #region GetById Tests

        [Fact]
        public async Task Get_ReturnsALunch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var lunch = GetSampleLunch(id);

            var classUnderTest = new LunchController(_lunchService.Object);
            _lunchService.Setup(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(lunch);

            // Act
            var result = await classUnderTest.Get(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var lunchResult = Assert.IsType<Lunch>(okResult.Value);
            Assert.True(Equals(lunchResult, lunch));

            _lunchService.Verify(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenIdNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var classUnderTest = new LunchController(_lunchService.Object);
            _lunchService.Setup(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(null as Lunch);

            // Assert
            var result = await classUnderTest.Get(id);

            // Act
            Assert.IsType<NotFoundResult>(result);

            _lunchService.Verify(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_ReturnsCreated()
        {
            var classUnderTest = new LunchController(_lunchService.Object);
            var lunch = GetSampleLunch();

            var lunchDto = new InputLunchDto
            {
                Date = lunch.Date,
                MealId = lunch.MealId
            };

            _lunchService.Setup(a => a.CreateAsync(
                It.Is<Lunch>(l => l.MealId == lunchDto.MealId && l.Date == lunchDto.Date)))
                .ReturnsAsync(lunch);

            var result = await classUnderTest.Create(lunchDto);

            var createdResponse = Assert.IsType<CreatedAtActionResult>(result);
            var resultLunch = Assert.IsType<LunchDto>(createdResponse.Value);

            Assert.Equal(lunch.MealId, resultLunch.MealId);
            Assert.Equal(lunch.Date, resultLunch.Date);

            _lunchService.Verify(a => a.CreateAsync(
                It.Is<Lunch>(l => l.MealId == lunchDto.MealId && l.Date == lunchDto.Date)), Times.Once);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenInputIsNull()
        {
            var classUnderTest = new LunchController(_lunchService.Object);

            var result = await classUnderTest.Create(null);

            Assert.IsType<BadRequestResult>(result);
            _lunchService.Verify(a => a.CreateAsync(It.IsAny<Lunch>()), Times.Never);
        }

        #endregion

        #region Update Test

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            var classUnderTest = new LunchController(_lunchService.Object);

            var id = Guid.NewGuid();
            var lunch = GetSampleLunch(id);
            var lunchDto = new InputLunchDto
            {
                Date = lunch.Date,
                MealId = lunch.MealId
            };

            _lunchService.Setup(a => a.UpdateAsync(
                It.Is<Lunch>(l =>
                        l.MealId == lunchDto.MealId
                    && l.Date == lunchDto.Date
                    && l.Id == id)))
                .ReturnsAsync(lunch);

            var result = await classUnderTest.Update(id, lunchDto);

            Assert.IsType<NoContentResult>(result);

            _lunchService.Verify(a => a.UpdateAsync(
                It.Is<Lunch>(l =>
                        l.MealId == lunchDto.MealId
                    && l.Date == lunchDto.Date
                    && l.Id == id)), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenDtoIsNull()
        {
            var classUnderTest = new LunchController(_lunchService.Object);

            var result = await classUnderTest.Update(Guid.NewGuid(), null);

            Assert.IsType<BadRequestResult>(result);
            _lunchService.Verify(a => a.UpdateAsync(It.IsAny<Lunch>()), Times.Never);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            var classUnderTest = new LunchController(_lunchService.Object);
            var id = Guid.NewGuid();
            _lunchService.Setup(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(1);

            var result = await classUnderTest.Delete(id);

            Assert.IsType<NoContentResult>(result);

            _lunchService.Verify(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdNotExist()
        {
            var classUnderTest = new LunchController(_lunchService.Object);

            var id = Guid.NewGuid();
            _lunchService.Setup(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(0);

            var result = await classUnderTest.Delete(id);

            Assert.IsType<NotFoundResult>(result);
            _lunchService.Verify(s => s.DeleteByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        #endregion

        #region Private Methods

        private Lunch GetSampleLunch(Guid? id = null)
        {
            return new Lunch
            {
                Date = DateTime.Now,
                Id = id ?? Guid.NewGuid(),
                MealId = Guid.NewGuid()
            };
        }

        private bool Equals(Lunch lunch1, Lunch lunch2)
        {
            return lunch1.Id == lunch2.Id
                && lunch1.MealId == lunch2.MealId
                && lunch1.Date == lunch2.Date;
        }

        #endregion
    }
}

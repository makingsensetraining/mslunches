using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services;
using Xunit;

namespace MSLunches.Domain.Tests.Services
{
    public class MealServiceTests
    {
        #region Members

        private readonly Mock<WebApiCoreLunchesContext> _context;

        #endregion

        #region Constructors

        public MealServiceTests()
        {
            _context = new Mock<WebApiCoreLunchesContext>();
        }

        #endregion

        #region GetById Tests

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMeal()
        {
            // Arrange
            var service = new MealService(_context.Object);
            var meal = GetADefaultMeal();
            _context.Setup(x => x.Meals.FindAsync(It.Is<Guid>(y => y == meal.Id)))
                 .ReturnsAsync(meal)
                 .Verifiable();

            // Act
            var retrievedMeal = await service.GetByIdAsync(meal.Id);

            // Assert
            Assert.NotNull(retrievedMeal);
            Assert.Equal(meal.Name, retrievedMeal.Name);
            Assert.Equal(meal.TypeId, retrievedMeal.TypeId);
            _context.Verify(c => c.Meals.FindAsync(meal.Id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull()
        {
            // Arrange 
            var service = new MealService(_context.Object);
            var id = Guid.NewGuid();
            _context.Setup(x => x.Meals.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync((Meal)null)
                 .Verifiable();

            //Act
            var retrievedMeal = await service.GetByIdAsync(id);

            // Assert
            Assert.Null(retrievedMeal);
            _context.Verify(c => c.Meals.FindAsync(id), Times.Once);
        }

        #endregion

        #region Detele Tests

        [Fact]
        public async void Delete_ShouldDeleteMeal()
        {
            // Arrange
            var service = new MealService(_context.Object);
            var createdMeal = GetADefaultMeal();
            _context.Setup(x => x.Meals.FindAsync(It.Is<Guid>(y => y == createdMeal.Id)))
                 .ReturnsAsync(createdMeal)
                 .Verifiable();
            _context.Setup(x => x.Meals.Remove(It.Is<Meal>(y => y.Id == createdMeal.Id)))
                 .Verifiable();
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

            // Act
            var affectedRows = await service.DeleteByIdAsync(createdMeal.Id);

            // Assert
            Assert.True(affectedRows > 0);
            _context.Verify(x => x.Meals.Remove(It.Is<Meal>(y => y.Id == createdMeal.Id)), Times.Once);
        }

        [Fact]
        public async void Delete_MealNotFound()
        {
            // Arrange  
            var service = new MealService(_context.Object);
            var id = Guid.NewGuid();
            _context.Setup(x => x.Meals.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync((Meal)null)
                 .Verifiable();

            //Act
            var affectedRows = await service.DeleteByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Equal(0, affectedRows);
            _context.Verify(x => x.Meals.Remove(It.Is<Meal>(y => y.Id == id)), Times.Never);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async void Update_ShouldUpdateIfMealExists()
        {
            // Arrange
            var service = new MealService(_context.Object);
            var meal = GetADefaultMeal();
            _context.Setup(x => x.Meals.FindAsync(It.Is<Guid>(y => y == meal.Id)))
                 .ReturnsAsync(meal)
                 .Verifiable();
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

            // Act
            var result = await service.UpdateAsync(meal);

            // Assert
            Assert.NotNull(result);
            _context.Verify(x => x.Meals.FindAsync(It.Is<Guid>(y => y == meal.Id)), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void Update_ReturnsNull_WhenMealNotExists()
        {
            // Arrange
            var service = new MealService(_context.Object);
            var createdMeal = GetADefaultMeal();
            _context.Setup(a => a.Meals.FindAsync(It.Is<Guid>(g => g == createdMeal.Id)))
                    .ReturnsAsync((Meal)null);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            // Act
            var result = await service.UpdateAsync(createdMeal);

            // Assert
            Assert.Null(result);
            _context.Verify(x => x.Meals.FindAsync(It.Is<Guid>(y => y == createdMeal.Id)), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Private Methods

        private Meal GetADefaultMeal(Guid? id = null)
        {
            return new Meal
            {
                Id = id ?? Guid.NewGuid(),
                TypeId = 1,
                Name = "Test"
            };
        }

        #endregion
    }
}

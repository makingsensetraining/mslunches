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
    public class LunchServiceTests
    {
        #region Members

        private readonly Mock<WebApiCoreLunchesContext> _context;

        #endregion

        #region Constructors

        public LunchServiceTests()
        {
            _context = new Mock<WebApiCoreLunchesContext>();
        }

        #endregion

        #region GetById Tests

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLunch()
        {
            // Arrange
            var service = new LunchService(_context.Object);
            var lunch = GetADefaultLunch();
            _context.Setup(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == lunch.Id)))
                 .ReturnsAsync(lunch)
                 .Verifiable();

            // Act
            var retrievedLunch = await service.GetByIdAsync(lunch.Id);

            // Assert
            Assert.NotNull(retrievedLunch);
            Assert.Equal(lunch.LunchName, retrievedLunch.LunchName);
            Assert.Equal(lunch.LunchTypeId, retrievedLunch.LunchTypeId);
            _context.Verify(c => c.Lunches.FindAsync(lunch.Id), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull()
        {
            // Arrange 
            var service = new LunchService(_context.Object);
            var id = Guid.NewGuid();
            _context.Setup(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync((Lunch)null)
                 .Verifiable();

            //Act
            var retrievedLunch = await service.GetByIdAsync(id);

            // Assert
            Assert.Null(retrievedLunch);
            _context.Verify(c => c.Lunches.FindAsync(id), Times.Once);
        }

        #endregion

        #region Detele Tests

        [Fact]
        public async void Delete_ShouldDeleteLunch()
        {
            // Arrange
            var service = new LunchService(_context.Object);
            var createdLunch = GetADefaultLunch();
            _context.Setup(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == createdLunch.Id)))
                 .ReturnsAsync(createdLunch)
                 .Verifiable();
            _context.Setup(x => x.Lunches.Remove(It.Is<Lunch>(y => y.Id == createdLunch.Id)))
                 .Verifiable();
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

            // Act
            var affectedRows = await service.DeleteByIdAsync(createdLunch.Id);

            // Assert
            Assert.True(affectedRows > 0);
            _context.Verify(x => x.Lunches.Remove(It.Is<Lunch>(y => y.Id == createdLunch.Id)), Times.Once);
        }

        [Fact]
        public async void Delete_LunchNotFound()
        {
            // Arrange  
            var service = new LunchService(_context.Object);
            var id = Guid.NewGuid();
            _context.Setup(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync((Lunch)null)
                 .Verifiable();

            //Act
            var affectedRows = await service.DeleteByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Equal(0, affectedRows);
            _context.Verify(x => x.Lunches.Remove(It.Is<Lunch>(y => y.Id == id)), Times.Never);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async void Update_ShouldUpdateIfLunchExists()
        {
            // Arrange
            var service = new LunchService(_context.Object);
            var lunch = GetADefaultLunch();
            _context.Setup(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == lunch.Id)))
                 .ReturnsAsync(lunch)
                 .Verifiable();
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);

            // Act
            var result = await service.UpdateAsync(lunch);

            // Assert
            Assert.NotNull(result);
            _context.Verify(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == lunch.Id)), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async void Update_ReturnsNull_WhenLunchNotExists()
        {
            // Arrange
            var service = new LunchService(_context.Object);
            var createdLunch = GetADefaultLunch();
            _context.Setup(a => a.Lunches.FindAsync(It.Is<Guid>(g => g == createdLunch.Id)))
                    .ReturnsAsync((Lunch)null);
            _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                    .ReturnsAsync(1);

            // Act
            var result = await service.UpdateAsync(createdLunch);

            // Assert
            Assert.Null(result);
            _context.Verify(x => x.Lunches.FindAsync(It.Is<Guid>(y => y == createdLunch.Id)), Times.Once);
            _context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Private Methods

        private static Lunch GetADefaultLunch(Guid? id = null)
        {
            return new Lunch
            {
                Id = id ?? Guid.NewGuid(),
                LunchTypeId = 1,
                LunchName = "Test"
            };
        }

        #endregion
    }
}

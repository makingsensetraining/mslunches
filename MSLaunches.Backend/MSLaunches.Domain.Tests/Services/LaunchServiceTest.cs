using Microsoft.EntityFrameworkCore;
using Moq;
using MSLunches.Data.EF;
using MSLunches.Data.Models;
using MSLunches.Domain.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Seed.Domain.Tests
{
    public class LunchServiceTests
    {
        #region GetById Tests

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLunch()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var createdLunch = GetADefaultLunch();
            context.Setup(x => x.Lunches).Returns(Lunches.Object);
            Lunches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == createdLunch.Id)))
                 .ReturnsAsync(createdLunch)
                 .Verifiable();
            var LunchService = new LunchService(context.Object);

            // Act
            var retrievedLunch = await LunchService.GetByIdAsync(createdLunch.Id);

            // Assert
            Assert.NotNull(retrievedLunch);
            Assert.Equal(createdLunch.LunchName, retrievedLunch.LunchName);
            Assert.Equal(createdLunch.Id, retrievedLunch.Id);
            context.VerifyAll();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var id = Guid.NewGuid();
            Lunch Lunch = null;
            context.Setup(x => x.Lunches)
                   .Returns(Lunches.Object);
            Lunches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync(Lunch)
                 .Verifiable();
            var LunchService = new LunchService(context.Object);

            //Act
            var retrievedLunch = await LunchService.GetByIdAsync(id);

            // Assert
            Assert.Null(retrievedLunch);
            context.VerifyAll();
        }

        #endregion

        #region Detele Tests

        [Fact]
        public async void Delete_ShouldDeleteLunch()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var createdLunch = GetADefaultLunch();
            context.Setup(x => x.Lunches)
                   .Returns(Lunches.Object);
            Lunches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == createdLunch.Id)))
                 .ReturnsAsync(createdLunch)
                 .Verifiable();
            Lunches.Setup(x => x.Remove(It.Is<Lunch>(y => y.Id == createdLunch.Id)))
                 .Verifiable();
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var LunchService = new LunchService(context.Object);

            // Act
            var affectedRows = await LunchService.DeleteByIdAsync(createdLunch.Id);

            // Assert
            Assert.True(affectedRows > 0);
            Lunches.Verify(x => x.Remove(It.Is<Lunch>(y => y.Id == createdLunch.Id)), Times.Once);
            context.VerifyAll();
        }

        [Fact]
        public async void Delete_LunchNotFound()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var id = Guid.NewGuid();
            Lunch Lunch = null;
            context.Setup(x => x.Lunches)
                   .Returns(Lunches.Object);
            Lunches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync(Lunch)
                 .Verifiable();
            var LunchService = new LunchService(context.Object);

            //Act
            var affectedRows = await LunchService.DeleteByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Equal(0, affectedRows);
            Lunches.Verify(x => x.Remove(It.Is<Lunch>(y => y.Id == id)), Times.Never);
            context.VerifyAll();
        }

        #endregion

        #region Update Tests

        [Fact]
        public async void Update_ShouldUpdateIfLunchExists()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var Lunch = GetADefaultLunch();
            context.Setup(x => x.Lunches)
                   .Returns(Lunches.Object);
            Lunches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == Lunch.Id)))
                 .ReturnsAsync(Lunch)
                 .Verifiable();
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var LunchService = new LunchService(context.Object);

            // Act
            int affectedRows = await LunchService.UpdateAsync(Lunch);

            // Assert
            Assert.True(affectedRows > 0);
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            context.VerifyAll();
        }

        [Fact]
        public async void Update_ShouldReturnZeroIfLunchNotExists()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var createdLunch = GetADefaultLunch();
            context.Setup(x => x.Lunches)
                   .Returns(Lunches.Object);

            var id = Guid.NewGuid();
            Lunches.Setup(a => a.FindAsync(It.Is<Guid>(g => g == id), It.IsAny<CancellationToken>()))
                 .Returns<Lunch>(null);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var LunchService = new LunchService(context.Object);

            // Act
            var affectedRows = await LunchService.UpdateAsync(createdLunch);

            // Assert
            Assert.Equal(0, affectedRows);
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async void Create_ShoulReturnOneIfCreated()
        {
            // Arrange
            var context = new Mock<WebApiCoreLunchesContext>();
            var Lunches = new Mock<DbSet<Lunch>>();
            var createdLunch = GetADefaultLunch();
            context.Setup(x => x.Lunches)
                   .Returns(Lunches.Object);
            Lunches.Setup(x => x.Add(It.Is<Lunch>(y => y.Id == createdLunch.Id)))
                 .Verifiable();
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var LunchService = new LunchService(context.Object);
            // Act
            var affectedRows = await LunchService.CreateAsync(createdLunch);
            // Assert
            Assert.Equal(1, affectedRows);
            context.VerifyAll();
            Lunches.Verify(x => x.Add(It.Is<Lunch>(y => y.Id == createdLunch.Id)), Times.Once);
        }

        #endregion

        #region Private Methods

        private static Lunch GetADefaultLunch(Guid? id = null)
        {
            var sanitizedId = id ?? Guid.NewGuid();
            var LunchType = new LunchType { Id = 2, Description = "Light" };

            return new Lunch
            {
                CreatedBy = "System",
                CreatedOn = DateTime.Now,
                LunchName = "Crepes de verdura y calabza",
                Id = Guid.NewGuid(),
                LunchType = LunchType
            };
        }

        #endregion
    }
}

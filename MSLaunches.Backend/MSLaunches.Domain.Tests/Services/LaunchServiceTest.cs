using Microsoft.EntityFrameworkCore;
using Moq;
using MSLaunches.Data.EF;
using MSLaunches.Data.Models;
using MSLaunches.Domain.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Seed.Domain.Tests
{
    public class LaunchServiceTests
    {
        #region GetById Tests

        [Fact]
        public async Task GetByIdAsync_ShouldReturnLaunch()
        {
            // Arrange
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var createdLaunch = GetADefaultLaunch();
            context.Setup(x => x.Launches).Returns(launches.Object);
            launches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == createdLaunch.Id)))
                 .ReturnsAsync(createdLaunch)
                 .Verifiable();
            var launchService = new LaunchService(context.Object);

            // Act
            var retrievedLaunch = await launchService.GetByIdAsync(createdLaunch.Id);

            // Assert
            Assert.NotNull(retrievedLaunch);
            Assert.Equal(createdLaunch.LaunchName, retrievedLaunch.LaunchName);
            Assert.Equal(createdLaunch.LaunchDescription, retrievedLaunch.LaunchDescription);
            Assert.Equal(createdLaunch.LaunchTypeId, retrievedLaunch.LaunchTypeId);
            context.VerifyAll();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull()
        {
            // Arrange
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var id = Guid.NewGuid();
            Launch launch = null;
            context.Setup(x => x.Launches)
                   .Returns(launches.Object);
            launches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync(launch)
                 .Verifiable();
            var launchService = new LaunchService(context.Object);

            //Act
            var retrievedLaunch = await launchService.GetByIdAsync(id);

            // Assert
            Assert.Null(retrievedLaunch);
            context.VerifyAll();
        }

        #endregion

        #region Detele Tests

        [Fact]
        public async void Delete_ShouldDeleteLaunch()
        {
            // Arrange
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var createdLaunch = GetADefaultLaunch();
            context.Setup(x => x.Launches)
                   .Returns(launches.Object);
            launches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == createdLaunch.Id)))
                 .ReturnsAsync(createdLaunch)
                 .Verifiable();
            launches.Setup(x => x.Remove(It.Is<Launch>(y => y.Id == createdLaunch.Id)))
                 .Verifiable();
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var launchService = new LaunchService(context.Object);

            // Act
            var affectedRows = await launchService.DeleteByIdAsync(createdLaunch.Id);

            // Assert
            Assert.True(affectedRows > 0);
            launches.Verify(x => x.Remove(It.Is<Launch>(y => y.Id == createdLaunch.Id)), Times.Once);
            context.VerifyAll();
        }

        [Fact]
        public async void Delete_LaunchNotFound()
        {
            // Arrange
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var id = Guid.NewGuid();
            Launch launch = null;
            context.Setup(x => x.Launches)
                   .Returns(launches.Object);
            launches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == id)))
                 .ReturnsAsync(launch)
                 .Verifiable();
            var launchService = new LaunchService(context.Object);

            //Act
            var affectedRows = await launchService.DeleteByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Equal(0, affectedRows);
            launches.Verify(x => x.Remove(It.Is<Launch>(y => y.Id == id)), Times.Never);
            context.VerifyAll();
        }

        #endregion

        #region Update Tests

        [Fact]
        public async void Update_ShouldUpdateIfLaunchExists()
        {
            // Arrange
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var launch = GetADefaultLaunch();
            context.Setup(x => x.Launches)
                   .Returns(launches.Object);
            launches.Setup(x => x.FindAsync(It.Is<Guid>(y => y == launch.Id)))
                 .ReturnsAsync(launch)
                 .Verifiable();
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var launchService = new LaunchService(context.Object);

            // Act
            int affectedRows = await launchService.UpdateAsync(launch);

            // Assert
            Assert.True(affectedRows > 0);
            context.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            context.VerifyAll();
        }

        [Fact]
        public async void Update_ShouldReturnZeroIfLaunchNotExists()
        {
            // Arrange
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var createdLaunch = GetADefaultLaunch();
            context.Setup(x => x.Launches)
                   .Returns(launches.Object);

            var id = Guid.NewGuid();
            launches.Setup(a => a.FindAsync(It.Is<Guid>(g => g == id), It.IsAny<CancellationToken>()))
                 .Returns<Launch>(null);
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var launchService = new LaunchService(context.Object);

            // Act
            var affectedRows = await launchService.UpdateAsync(createdLaunch);

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
            var context = new Mock<WebApiCoreLaunchesContext>();
            var launches = new Mock<DbSet<Launch>>();
            var createdLaunch = GetADefaultLaunch();
            context.Setup(x => x.Launches)
                   .Returns(launches.Object);
            launches.Setup(x => x.Add(It.Is<Launch>(y => y.Id == createdLaunch.Id)))
                 .Verifiable();
            context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                   .ReturnsAsync(1);
            var launchService = new LaunchService(context.Object);
            // Act
            var affectedRows = await launchService.CreateAsync(createdLaunch);
            // Assert
            Assert.Equal(1, affectedRows);
            context.VerifyAll();
            launches.Verify(x => x.Add(It.Is<Launch>(y => y.Id == createdLaunch.Id)), Times.Once);
        }

        #endregion

        #region Private Methods

        private static Launch GetADefaultLaunch(Guid? id = null)
        {
            var sanitizedId = id ?? Guid.NewGuid();
            var launchType = new LaunchType { LaunchTypeId = 2, LaunchTypeDescription = "Light" };

            return new Launch
            {
                CreatedBy = "System",
                CreatedOn = DateTime.Now,
                LaunchName = "Crepes de verdura y calabza",
                LaunchTypeId = 2,
                LaunchType = launchType
            };
        }

        #endregion
    }
}

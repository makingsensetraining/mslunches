using Microsoft.AspNetCore.Mvc;
using Moq;
using MSLunches.Api.Controllers;
using MSLunches.Api.Models.Request;
using MSLunches.Api.Models.Response;
using MSLunches.Data.Models;
using MSLunches.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MSLunches.Api.Tests.Controllers
{
    public class UserLunchControllerTests
    {
        private Mock<IUserLunchService> _userLunchService;

        public UserLunchControllerTests()
        {
            _userLunchService = new Mock<IUserLunchService>();
        }

        #region GetAll Tests

        [Fact]
        public async Task GetAll_ReturnsListOfUserLunches()
        {
            var listOfUserLunch = new List<UserLunch>
            {
                GetSampleUserLunch(),
                GetSampleUserLunch()
            };

            var userId = "userid1";
            var classUnderTest = new UserLunchController(_userLunchService.Object);
            _userLunchService.Setup(a => a.GetAsync(It.Is<string>(s => s == userId)))
                .ReturnsAsync(listOfUserLunch);

            var result = await classUnderTest.GetAll(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var listResult = Assert.IsAssignableFrom<IEnumerable<UserLunchDto>>(okResult.Value);

            foreach (var userLunch in listOfUserLunch)
            {
                Assert.Contains(listResult, lunch => Equals(userLunch, lunch));
            }

            _userLunchService.Verify(a => a.GetAsync(It.Is<string>(s => s == userId)), Times.Once);
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task Get_ReturnsOkResult()
        {
            var classUnderTest = new UserLunchController(_userLunchService.Object);
            var id = Guid.NewGuid();
            var userLunch = GetSampleUserLunch();
            _userLunchService.Setup(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(userLunch);

            var result = await classUnderTest.Get(id);
            var okresult = Assert.IsType<OkObjectResult>(result);
            var userLunchResult = Assert.IsType<UserLunchDto>(okresult.Value);
            Assert.True(Equals(userLunch, userLunchResult));
            _userLunchService.Verify(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenIdNotExist()
        {
            var classUnderTest = new UserLunchController(_userLunchService.Object);
            var id = Guid.NewGuid();
            _userLunchService.Setup(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(null as UserLunch);

            var result = await classUnderTest.Get(id);
            Assert.IsType<NotFoundResult>(result);
            _userLunchService.Verify(a => a.GetByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        #endregion

        #region Create Tests

        [Fact]
        public async Task Create_ReturnsCreated()
        {
            var userId = "userId1234";
            var userLunch = GetSampleUserLunch(userId: userId);
            var userLunchDto = new UserLunchRequest
            {
                Approved = userLunch.Approved,
                LunchId = userLunch.LunchId,
                UserId = userId
            };

            var classUnderTest = new UserLunchController(_userLunchService.Object);

            _userLunchService.Setup(a => a.GetUserLunchByUserAndLunchIdAsync(
                It.Is<string>(u => u == userId),
                It.Is<Guid>(g => g == userLunch.LunchId)))
                .ReturnsAsync(null as UserLunch);

            _userLunchService.Setup(a => a.CreateAsync(It.Is<UserLunch>(u => Equals(u, userLunchDto))))
                .ReturnsAsync(userLunch);

            var result = await classUnderTest.Create(userId, userLunchDto);

            var okresult = Assert.IsType<CreatedAtActionResult>(result);
            var createdLunch = Assert.IsType<UserLunchResponse>(okresult.Value);
            Assert.True(Equals(createdLunch, userLunchDto));
            _userLunchService.Verify(a => a.GetUserLunchByUserAndLunchIdAsync(
                It.Is<string>(u => u == userId),
                It.Is<Guid>(g => g == userLunch.LunchId)), Times.Once);

            _userLunchService.Verify(a => a.CreateAsync(It.Is<UserLunch>(u => Equals(u, userLunchDto))), Times.Once);
        }

        [Fact]
        public async Task Get_Returns422_WhenUserLunchExist()
        {
            var classUnderTest = new UserLunchController(_userLunchService.Object);
            var userId = "userid1234";
            var userLunchDto = new UserLunchRequest
            {
                Approved = true,
                LunchId = Guid.NewGuid(),
                UserId = userId
            };
            var userLunch = GetSampleUserLunch(lunchId: userLunchDto.LunchId, userId: userId);

            _userLunchService.Setup(a => a.GetUserLunchByUserAndLunchIdAsync(
                It.Is<string>(u => u == userId),
                It.Is<Guid>(g => g == userLunch.LunchId)))
                .ReturnsAsync(userLunch);

            var result = await classUnderTest.Create(userId, userLunchDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(422, objectResult.StatusCode);

            _userLunchService.Verify(a => a.GetUserLunchByUserAndLunchIdAsync(
                It.Is<string>(u => u == userId),
                It.Is<Guid>(g => g == userLunchDto.LunchId)), Times.Once);

            _userLunchService.Verify(a => a.CreateAsync(It.IsAny<UserLunch>()), Times.Never);
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task Update_ReturnsNoContent()
        {
            var classUnderTest = new UserLunchController(_userLunchService.Object);
            var id = Guid.NewGuid();
            var userId = "userId123";
            var lunch = GetSampleUserLunch(id: id, userId: userId);
            var inputLunch = new UserLunchRequest()
            {
                Approved = lunch.Approved,
                LunchId = lunch.LunchId,
                UserId = lunch.UserId
            };

            _userLunchService.Setup(a => a.UpdateAsync(
                It.Is<UserLunch>(u => Equals(u, inputLunch))))
                .ReturnsAsync(lunch);

            var result = await classUnderTest.Update(userId, id, inputLunch);

            Assert.IsType<NoContentResult>(result);
            _userLunchService.Verify(a => a.UpdateAsync(
                It.Is<UserLunch>(u => Equals(u, inputLunch))), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenIdNotExist()
        {
            var classUnderTest = new UserLunchController(_userLunchService.Object);
            var id = Guid.NewGuid();
            var userId = "userId123";
            var inputLunch = new UserLunchRequest
            {
                Approved = true,
                LunchId = Guid.NewGuid(),
                UserId = userId
            };

            _userLunchService.Setup(a => a.UpdateAsync(
                It.Is<UserLunch>(u => Equals(u, inputLunch))))
                .ReturnsAsync(null as UserLunch);

            var result = await classUnderTest.Update(userId, id, inputLunch);

            Assert.IsType<NotFoundResult>(result);
            _userLunchService.Verify(a => a.UpdateAsync(
                It.Is<UserLunch>(u => Equals(u, inputLunch))), Times.Once);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            var classUnderTest = new UserLunchController(_userLunchService.Object);

            _userLunchService.Setup(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(1);

            var result = await classUnderTest.Delete(id);

            Assert.IsType<NoContentResult>(result);
            _userLunchService.Verify(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdNotExist()
        {
            var id = Guid.NewGuid();
            var classUnderTest = new UserLunchController(_userLunchService.Object);

            _userLunchService.Setup(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)))
                .ReturnsAsync(0);

            var result = await classUnderTest.Delete(id);

            Assert.IsType<NotFoundResult>(result);
            _userLunchService.Verify(a => a.DeleteByIdAsync(It.Is<Guid>(g => g == id)), Times.Once);
        }

        #endregion

        #region Private methods

        private UserLunch GetSampleUserLunch(Guid? id = null, Guid? lunchId = null, string userId = "") =>
            new UserLunch
            {
                Id = id ?? Guid.NewGuid(),
                Approved = true,
                LunchId = lunchId ?? Guid.NewGuid(),
                UserId = string.IsNullOrEmpty(userId) ? "randomUserId" : userId
            };

        private bool Equals(UserLunch us1, UserLunch us2) =>
            us1.Approved == us2.Approved
            && us1.Id == us2.Id
            && us1.LunchId == us2.LunchId
            && us1.UserId == us2.UserId;

        private bool Equals(UserLunch us1, UserLunchRequest us2) =>
            us1.Approved == us2.Approved
            && us1.LunchId == us2.LunchId
            && us1.UserId == us2.UserId;

        private bool Equals(UserLunch us1, UserLunchDto us2) =>
            us1.Approved == us2.Approved
            && us1.LunchId == us2.LunchId
            && us1.UserId == us2.UserId;

        private bool Equals(UserLunchDto us1, InputUserLunchDto us2) =>
            us1.Approved == us2.Approved
            && us1.LunchId == us2.LunchId
            && us1.UserId == us2.UserId;

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationService.Helpers;
using AuthenticationService.Messages;
using AuthenticationService.Messages.Request;
using AuthenticationService.Services;
using AuthenticationServiceTests.Mocks;
using Common.Models;

namespace AuthenticationServiceTests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private UserService _userService;
        public UserServiceTests()
        {
            _userService = new UserService(new MockUserDataAccess(), new MockConfiguration(),
                new MockLogger<UserService>());
        }

        [TestMethod]
        public void CreateUserAsync_AssertFalseWhenUserAlreadyExists()
        {
            var request = new UserRequest(){Email = "aa@email.com", Password = ""};
            var result = _userService.CreateUserAsync(request);
            Assert.IsFalse(result.Result.Success);
        }

        [TestMethod]
        public void CreateUserAsync_AssertTrueWhenUserDoesNotExists()
        {
            var request = new UserRequest() { Email = "aaqq@email.com", Password = "" };
            var result = _userService.CreateUserAsync(request);
            Assert.IsTrue(result.Result.Success);
        }

        [TestMethod]
        public void LoginUser_AssertFalseWhenUserDoesNotExists()
        {
            var request = new UserRequest() { Email = "aaqq@email.com", Password = "" };
            var result = _userService.LoginUser(request);
            Assert.IsFalse(result.Result.Success);
        }

        [TestMethod]
        public void LoginUser_AssertFalseWhenPasswordIsWrong()
        {
            var request = new UserRequest() { Email = "aaqq@email.com", Password = "" };
            var result = _userService.LoginUser(request);
            Assert.IsFalse(result.Result.Success);
        }

        [TestMethod]
        public void LoginUser_AssertTrueWhenPasswordIsCorrect()
        {
            var request = new UserRequest() { Email = "aa@email.com", Password = "Aa123456@" };
            var result = _userService.LoginUser(request);
            Assert.IsTrue(result.Result.Success);
        }


    }
}

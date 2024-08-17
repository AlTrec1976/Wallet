using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Wallet.BLL.Logic.Contracts.Users;
using Wallet.BLL.Logic.Users;
using Wallet.Common.Entities.User.DB;
using Wallet.Common.Entities.User.InputModels;
using Wallet.Common.Entities.UserModels.InputModels;
using Wallet.DAL.Repository.EF;
using Wallet.Tests.DI;

namespace Wallet.BLL.Tests
{
    public class UserLogicTests : BaseFixture
    {
        [SetUp]
        public void Setup()
        {
            //RegisterMock<ILogger<UserLogic>>();
        }

        [Test]
        public async Task CreateUser_NameEmpty_Shoud_ThrowArgumentException_Test()
        {
            //Arrange
            var userLogic = ResolveServices<IUserLogic>();
            var request = new UserCreateInputModel
            {
                Login = "test",
                Password = "test",
                Email = "test",
                Name = "",
                Surname = "test",
            };

            //var dao = RegisterMock<IEFUserRepository>();
            //dao.Setup(s => s.Create(It.IsAny<User>()));
            //var userInM = It.IsAny<UserCreateInputModel>();

            //Act

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await userLogic.CreateUserAsync(request));
        }

        [Test]
        public async Task CreateUser_NameEmpty_Shoud_ThrowArgumentException_Test1()
        {
            //Arrange
            var userLogic = ResolveServices<IUserLogic>();
            var request = new UserCreateInputModel
            {
                Login = "test",
                Password = "test",
                Email = "test",
                Name = "",
                Surname = "test",
            };

            //var dao = RegisterMock<IEFUserRepository>();
            //dao.Setup(s => s.Create(It.IsAny<User>()));
            //var userInM = It.IsAny<UserCreateInputModel>();

            //Act

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await userLogic.CreateUserAsync(request));
        }

        [Test]
        public async Task CreateUser_Null_Shoud_ThrowNullReferenceException_Test()
        {
            var userLogic = ResolveServices<IUserLogic>();

            Assert.ThrowsAsync<NullReferenceException>(async () => await userLogic.CreateUserAsync(null));
        }

        [Test]
        public async Task UpdateUser_NameEmpty_Shoud_ThrowArgumentException_Test1()
        {
            var userLogic = ResolveServices<IUserLogic>();
            var request = new UserUpdateInputModel
            {
                Id = new Guid(),
                Login = "test",
                Password = "test",
                Email = "test",
                Name = "",
                Surname = "test",
            };

            Assert.ThrowsAsync<ArgumentException>(async () => await userLogic.UpdateUserAsync(request));
        }

        [Test]
        public async Task UpdateUser_Null_Shoud_ThrowNullReferenceException_Test()
        {
            var userLogic = ResolveServices<IUserLogic>();

            Assert.ThrowsAsync<NullReferenceException>(async () => await userLogic.UpdateUserAsync(null));
        }
    }
}
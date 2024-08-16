using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Wallet.BLL.Logic.Auth;
using Wallet.BLL.Logic.Contracts.Auth;
using Wallet.BLL.Logic.Contracts.Notififcation;
using Wallet.BLL.Logic.Contracts.Users;
using Wallet.BLL.Logic.Users;
using Wallet.DAL.Repository.EF;

namespace Wallet.Tests.DI
{
    public abstract class BaseFixture
    {
        private IServiceProvider _serviceProvider;
        private IServiceCollection _serviceCollection;

        protected Mock<T> RegisterMock<T>() where T : class
        {
            if (_serviceProvider is not null)
            {
                var service = _serviceProvider.GetService<T>();
                var mockService = Mock.Get(service);

                return mockService;
            }

            var mock = new Mock<T>();
            _serviceCollection.AddSingleton(mock.Object);

            return mock;
        }

        protected T ResolveServices<T>() where T : class
        {
            if (_serviceProvider is null)
            {
                _serviceProvider = _serviceCollection.BuildServiceProvider();
            }

            return _serviceProvider.GetService<T>();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            _serviceCollection = new ServiceCollection();
            _serviceCollection.AddSingleton<IUserLogic, UserLogic>();
            _serviceCollection.AddSingleton(Mock.Of<IEFUserRepository>());
            _serviceCollection.AddSingleton(Mock.Of<INotificationLogic>());
            _serviceCollection.AddSingleton(Mock.Of<IPasswordHasher>());
            _serviceCollection.AddSingleton(Mock.Of<IJwtProvider>());
            _serviceCollection.AddSingleton(Mock.Of<ILogger<UserLogic>>());
            _serviceCollection.AddLogging();

            _serviceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}
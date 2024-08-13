using Microsoft.Extensions.Logging;
using Wallet.BLL.Logic.Contracts.Users;
using Wallet.Common.Entities.User.InputModels;
using Wallet.Common.Entities.User.DB;
using Wallet.DAL.Repository;
using Wallet.DAL.Repository.Contracts;
using Wallet.DAL.Repository.EF;
using System.Transactions;
using System.Net.Mail;
using Wallet.BLL.Logic.Contracts.Auth;
using EmailService.Contracts;
using Wallet.BLL.Logic.Contracts.Notififcation;
using Wallet.BLL.Logic.Auth;
using static EmailServiceClientGrpcApp.EmailServiceGrpc;
using EmailServiceClientGrpcApp;

namespace Wallet.BLL.Logic.Users
{
    public class UserLogic : IUserLogic
    {

        private readonly IEFUserRepository _eFUserRepository;
        private readonly INotificationLogic _notification;
        private readonly ILogger<UserLogic> _logger;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;
        private readonly EmailServiceGrpcClient _emailService;

        public UserLogic(
            IEFUserRepository eFUserRepository,
            INotificationLogic notification,
            ILogger<UserLogic> logger,
            IPasswordHasher passwordHasher,
            EmailServiceGrpcClient emailService,
            IJwtProvider jwtProvider
            )
        {
            _eFUserRepository = eFUserRepository;
            _notification = notification;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
            _emailService = emailService;
        }

        public async Task CreateUserAsync(UserCreateInputModel userInputModel)
        {
            try
            {
                using(var transaction = new TransactionScope())
                ValidateUser(userInputModel);
                var _hashedPassword = _passwordHasher.Generate(userInputModel.Password);

                var user = new User
                {
                    Name = userInputModel.Name,
                    Email = userInputModel.Email,
                    Login = userInputModel.Login,
                    Password = _hashedPassword,
                    Surname = userInputModel.Surname
                };

                await _eFUserRepository.Create(user);
                _logger.LogInformation($"Id user: {user.Id}");

                // отправка через Kafka
                //var emailMsg = new EmailServiceMessage { EmailFrom = "somemail@altrec.ru", EmailTo = user.Email, MessageBody = "Вы зарегистрированы" };
                //await _notification.SendAsync(emailMsg);

                // отправка через gRPC
                var emailMsg = new EmailRequest { EmailFrom = "somemail@altrec.ru", EmailTo = user.Email, MessageBody = "Вы зарегистрированы" };
                await _emailService.SendAsync(emailMsg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка при создании User");
                throw;
            }
        }


        public async Task<List<User>> Get()
        {
            return await _eFUserRepository.GetAllAsync();
        }

        public async Task<User> Get(Guid id)
        {
            var user = await _eFUserRepository.GetAsync(id);
            user.Name = "Tom";
            await _eFUserRepository.SomeWorkAsync();
            return user;
        }

        private void ValidateUser(UserCreateInputModel user)
        {
            List<string> exceptionsMessages = new List<string>();

            if (user == null)
            {
                exceptionsMessages.Add("User не может быть null");
            }

            if (string.IsNullOrEmpty(user.Name))
            {
                exceptionsMessages.Add("Namee не может быть null или пустым");
            }

            if (exceptionsMessages.Any())
            {
                foreach (var exception in exceptionsMessages)
                {
                    _logger.LogError(exception);
                }
                throw new ArgumentException();
            }
        }

        public async Task<string> LoginAsync(UserCreateInputModel userInputModel)
        {
            try
            {
                var user = await _eFUserRepository.GetByUserLoginAsync(userInputModel.Login);

                if (user is null)
                {
                    return "Нет записи";
                }

                //проверка пароля
                var result = _passwordHasher.Verify(userInputModel.Password, user.Password);
                if (result is false)
                {
                    throw new Exception("Пароль введен не верно");
                }

                var token = _jwtProvider.GenerateToken(user);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в LoginAsync");
                throw;
            }
        }
    }
}

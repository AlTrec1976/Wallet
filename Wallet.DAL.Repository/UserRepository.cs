using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Wallet.Common.Entities.Authz;
using Wallet.Common.Entities.User.DB;
using Wallet.DAL.Repository.Contracts;

namespace Wallet.DAL.Repository
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(ILogger<UserRepository> logger, IConfiguration configuration) 
            : base(logger, configuration)
        {
            _logger = logger;
        }

        public async Task AddAsync(User user)
        {
            try
            {
                var sql = "SELECT * FROM public.post_user(@_login, @_password, @_name, @_surname)";
                var param = new
                {
                    _login = user.Login,
                    _password = user.Password,
                    _name = user.Name,
                    _surname = user.Surname
                };

                await ExecuteAsync(sql, param);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении юзера");
                throw;
            }
        }

        public async Task<List<User>> GetAsync()
        {
            try
            {
                var sql = "SELECT userid AS Id, userlogin AS Login, userpassword AS Password FROM public.users";

                var users = await QueryAsync<User>(sql);
                return users.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка пользователей");
                throw;
            }
        }

        public async Task<User?> GetAsync(Guid id)
        {
            try
            {
                var sql = $"SELECT userid AS Id, userlogin AS Login, userpassword AS Password FROM public.users WHERE userid = {id}";

                return await QuerySingleAsync<User>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя");
                throw;
            }
        }

        public async Task<User?> GetByUserLoginAsync(string login)
        {
            try
            {
                var sql = $"SELECT userid AS Id, userlogin AS Login, userpassword AS Password FROM public.users WHERE userlogin = {login}";

                return await QuerySingleAsync<User>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении пользователя");
                throw;
            }
        }

        public async Task<HashSet<Permission>> GetUserPermission(Guid userID)
        {
            var sql = @"SELECT * FROM public.get_permission(@id)";
            var permission = new Permission();

            var param = new { id = userID };

            var hst = new HashSet<Permission>(await QueryAsync<Permission>(sql, param));


            return hst;
        }

    }
}
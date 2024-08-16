using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.Common.Entities.User.DB;

namespace Wallet.DAL.Repository.EF
{
    public interface IEFUserRepository
    {
        Task Create(User user);
        Task Update(User user);
        Task<List<User>> GetAllAsync();
        Task<User?> GetAsync(Guid userId);
        Task<User?> GetByUserLoginAsync(string userLogin);
        Task SomeWorkAsync();
    }
}

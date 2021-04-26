using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TG.Data;
using TG.Entity;

namespace TG.Interface
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<User>> GetUserList();
        Task<User> GetUser(int id);
        Task<User> GetTG(long id);
        Task Add(string firstname, string lastname, string username, long tgid);
        Task Create(UserEntity item);
        Task Update(User item);
        Task Delete(int id);
    }
}

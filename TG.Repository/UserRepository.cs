using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TG.Data;
using TG.Entity;
using TG.Interface;

namespace TG.Repository
{
    public class UserRepository : IUserRepository
    {
        private UserContext db;
        public UserRepository(UserContext context)
        {
            db = context;
        }
        public async Task<IEnumerable<User>> GetUserList()
        {
            UserEntity[] bigdata = await db.Users.ToArrayAsync();
            var users = from p in bigdata
                        select p;
            var data = users.Select(x => new User
            {
                Id = x.Id,
                FirstName= x.FirstName,
                LastName = x.LastName,
                Username = x.Username,
                TGId = x.TGId,
                Status = x.Status
            });
            return data;      
        }       
        public async Task<User> GetUser(int id)
        {
            UserEntity user = await db.Users.FirstOrDefaultAsync(x =>x.Id == id);
           if(user == null)
            {
                return null;
            }
            User result = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                TGId = user.TGId,
                Status = user.Status
            };
            return result;
        }
        public async Task<User> GetTG(long id)
        {
            UserEntity user = await db.Users.FirstOrDefaultAsync(x => x.TGId == id);
            if (user == null)
            {
                return null;
            }
            User result = new User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                TGId = user.TGId,
                Status = user.Status
            };
            return result;
        }
        public async Task Add(string firstname,string lastname,string username, long tgid)
        {
            UserEntity user = new UserEntity
            {
                FirstName = firstname,
                LastName = lastname,
                Username = username,
                TGId = tgid,
                Status = "Blocked"
            };
            await Create(user);
        }
        public async Task Create(UserEntity account)
        {           
            await db.Users.AddAsync(account);
            await db.SaveChangesAsync();
        }

        public async Task Update(User account)
        {
            var original = await db.Users.FindAsync(account.Id);
            db.Entry(original).CurrentValues.SetValues(account);
            await  db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            UserEntity account = await db.Users.FindAsync(id);
            if (account != null)
                await Task.Run(() => db.Users.Remove(account));
            await db.SaveChangesAsync();
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }       
    }
}

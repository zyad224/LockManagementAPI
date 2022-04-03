using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbApiContext _dbApiContext;

        public UserRepository(DbApiContext dbApiContext)
        {
            _dbApiContext = dbApiContext;

        }
        public async Task<User> AuthenticateUser(Email email, string password)
        {
            if (email == null || (string.IsNullOrEmpty(password)))
                throw new UserInvalidException("UserInvalidException");

            var userdb = await _dbApiContext.Users
                         .Where(user => user.Email.Value == email.Value && user.Password == password)          
                         .FirstOrDefaultAsync();

            if (userdb == null)
                throw new UserNotFoundException("UserNotFoundException");

            return userdb;

        }

        public async Task RegisterUser(User user)
        {
            if (user == null)
                throw new UserInvalidException("UserInvalidException");

            var userdb = await _dbApiContext.Users.Where(u => u.Email.Value == user.Email.Value).FirstOrDefaultAsync();

            if(userdb != null)
                throw new UserInvalidException("UserAlreadyExistException");

            await _dbApiContext.Users.AddAsync(user);
        }


        public async Task<User> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UserInvalidException("UserInvalidException");

            var userdb = await _dbApiContext.Users
                        .Where(u => u.UserId == userId) 
                        .Include(u => u.Locks)
                        .FirstOrDefaultAsync();

            if (userdb == null)
                throw new UserNotFoundException("UserNotFoundException");

            return userdb;
        }
    }
}

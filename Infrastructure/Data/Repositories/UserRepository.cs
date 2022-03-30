﻿using Domain.DomainExceptions;
using Domain.Entities;
using Domain.Interfaces;
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
        public async Task<User> GetUserById(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new UserInvalidException("UserInvalidException");

            return await _dbApiContext.Users.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task RegisterUser(User user)
        {
            if (user == null)
                throw new UserInvalidException("UserInvalidException");

            var userdb = await _dbApiContext.Users.Where(u => u.UserId == user.UserId).FirstOrDefaultAsync();

            if(userdb != null)
                throw new UserInvalidException("UserAlreadyExistException");

            await _dbApiContext.Users.AddAsync(user);
        }

        public async Task UpdateUserDetails(User user)
        {
            if (user == null)
                throw new UserInvalidException("UserInvalidException");
            
            var userdb = await _dbApiContext.Users.Where(u => u.UserId == user.UserId).FirstOrDefaultAsync();


            userdb.SetUserEmail(user.Email);
            userdb.SetUserName(user.Name);
            userdb.SetUserRole(user.Role);
        }
    }
}
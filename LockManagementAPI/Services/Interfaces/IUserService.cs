using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task RegisterUser(User user);
        public Task<User> AuthenticateUser(Email email, string password);
        public Task<User> GetUserById(string userId);
    }
}

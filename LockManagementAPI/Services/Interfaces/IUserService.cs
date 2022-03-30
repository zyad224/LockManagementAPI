using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        public Task RegisterUser(User user);
        public Task<User> AuthenticateUser(string email, string password);
        public Task UpdateUserDetails(User user);
    }
}

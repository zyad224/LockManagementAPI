using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task RegisterUser(User user);
        public Task<User> AuthenticateUser(string email, string password);
        public Task UpdateUserDetails(User user);

    }
}

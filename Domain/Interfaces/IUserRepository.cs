using Domain.Entities;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        public Task RegisterUser(User user);
        public Task<User> AuthenticateUser(Email email, string password);
        public Task<User> GetUserById(string userId);

    }
}

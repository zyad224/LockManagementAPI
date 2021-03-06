using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using LockManagementAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> AuthenticateUser(Email email, string password)
        {
            return await _unitOfWork.UserRepo.AuthenticateUser(email,password);

        }

        public async Task RegisterUser(User user)
        {
           await _unitOfWork.UserRepo.RegisterUser(user);
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _unitOfWork.UserRepo.GetUserById(userId);
        }

    }
}

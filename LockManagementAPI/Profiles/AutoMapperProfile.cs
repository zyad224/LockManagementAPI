using AutoMapper;
using Domain.Entities;
using LockManagementAPI.Dtos.LocksDto;
using LockManagementAPI.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockManagementAPI.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRegisterReqDto, User>();


            CreateMap<UserRegisterReqDto, User>()
                .ConstructUsing(x => new User
                (new Domain.ValueObjects.Name(x.FirstName,x.LastName),
                 new Domain.ValueObjects.Email(x.Email),
                 x.Role,
                 x.Password
                ));
            CreateMap<User, UserLoginRespDto>();

            CreateMap<Lock, LockResponseDto>();
            CreateMap<User, UserRegisterRespDto>();

             /*CreateMap<ProductTypeDto, ProductType>();
             CreateMap<ProductTypeQuantityDto, ProductTypeQuantity>();
             CreateMap<OrderRequestDto, Order>();

             CreateMap<ProductType, ProductTypeDto>();
             CreateMap<ProductTypeQuantity, ProductTypeQuantityDto>();
             CreateMap<Order, OrderResponseDto>();*/
        }
    }
}

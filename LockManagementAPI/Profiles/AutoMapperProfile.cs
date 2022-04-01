using AutoMapper;
using Domain.Entities;
using LockManagementAPI.Dtos.AuditsDto;
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

            ///User Mapping
            CreateMap<UserRegisterReqDto, User>()
                .ConstructUsing(x => new User
                (new Domain.ValueObjects.Name(x.FirstName,x.LastName),
                 new Domain.ValueObjects.Email(x.Email),
                 x.Role,
                 x.Password
                ));
            CreateMap<User, UserLoginRespDto>();
            CreateMap<User, UserRegisterRespDto>();

            ////Lock Mapping
            CreateMap<LockAddReqDto, Lock>()
              .ConstructUsing(x => new Lock
              (x.UserId,
               x.Description,
               x.HardwareId
              ));
            CreateMap<Lock, LockAddRespDto>();

            /////Audits Mapping
            CreateMap<Audit, AuditRespDto>();

        }
    }
}

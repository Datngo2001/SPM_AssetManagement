﻿using Rookie.AssetManagement.Contracts.Dtos.AuthDtos;
using Rookie.AssetManagement.Contracts.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rookie.AssetManagement.Business.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> AddAsync(UserCreateDto assetRequest, string location);
        Task<UserDto> UpdateAsnyc(int id, UserUpdateDto assetRequest);
    }
}

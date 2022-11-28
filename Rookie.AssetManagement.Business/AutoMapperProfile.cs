﻿using Rookie.AssetManagement.Contracts.Dtos.AuthDtos;
using Rookie.AssetManagement.Contracts.Dtos.UserDtos;
using Rookie.AssetManagement.Contracts.Dtos.AssetDtos;
using Rookie.AssetManagement.Contracts.Dtos.CategoryDtos;
using Rookie.AssetManagement.Contracts.Dtos.StateDtos;
using Rookie.AssetManagement.DataAccessor.Entities;
using Rookie.AssetManagement.DataAccessor.Data;

namespace Rookie.AssetManagement.Business
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            FromDataAccessorLayer();
            FromPresentationLayer();
            MapperAssetFromPresentationLayer();
        }

        private void FromPresentationLayer()
        {
            CreateMap<UserCreateDto, User>()
                .ForMember(d => d.IsDeleted, t => t.Ignore())
                .ForMember(d => d.StaffCode, t => t.Ignore())
                .ForMember(d => d.Location, t => t.Ignore())
                .ForMember(d => d.IsNewUser, t => t.Ignore())
                .ForMember(d => d.Id, t => t.Ignore())
                .ForMember(d => d.UserName, t => t.Ignore())
                .ForMember(d => d.NormalizedUserName, t => t.Ignore())
                .ForMember(d => d.Email, t => t.Ignore())
                .ForMember(d => d.NormalizedEmail, t => t.Ignore())
                .ForMember(d => d.EmailConfirmed, t => t.Ignore())
                .ForMember(d => d.PasswordHash, t => t.Ignore())
                .ForMember(d => d.PhoneNumber, t => t.Ignore())
                .ForMember(d => d.PhoneNumberConfirmed, t => t.Ignore())
                .ForMember(d => d.TwoFactorEnabled, t => t.Ignore())
                .ForMember(d => d.LockoutEnd, t => t.Ignore())
                .ForMember(d => d.LockoutEnabled, t => t.Ignore())
                .ForMember(d => d.AccessFailedCount, t => t.Ignore())
                .ForMember(d => d.SecurityStamp, t => t.Ignore())
                .ForMember(d => d.ConcurrencyStamp, t => t.Ignore());
            CreateMap<UserUpdateDto, User>()
                .ForMember(d => d.FirstName, t => t.Ignore())
                .ForMember(d => d.LastName, t => t.Ignore())
                .ForMember(d => d.IsDeleted, t => t.Ignore())
                .ForMember(d => d.StaffCode, t => t.Ignore())
                .ForMember(d => d.Location, t => t.Ignore())
                .ForMember(d => d.IsNewUser, t => t.Ignore())
                .ForMember(d => d.Id, t => t.Ignore())
                .ForMember(d => d.UserName, t => t.Ignore())
                .ForMember(d => d.NormalizedUserName, t => t.Ignore())
                .ForMember(d => d.Email, t => t.Ignore())
                .ForMember(d => d.NormalizedEmail, t => t.Ignore())
                .ForMember(d => d.EmailConfirmed, t => t.Ignore())
                .ForMember(d => d.PasswordHash, t => t.Ignore())
                .ForMember(d => d.PhoneNumber, t => t.Ignore())
                .ForMember(d => d.PhoneNumberConfirmed, t => t.Ignore())
                .ForMember(d => d.TwoFactorEnabled, t => t.Ignore())
                .ForMember(d => d.LockoutEnd, t => t.Ignore())
                .ForMember(d => d.LockoutEnabled, t => t.Ignore())
                .ForMember(d => d.AccessFailedCount, t => t.Ignore())
                .ForMember(d => d.SecurityStamp, t => t.Ignore())
                .ForMember(d => d.ConcurrencyStamp, t => t.Ignore());
            CreateMap<AssetCreateDto, Asset>()
                .ForMember(d => d.Id, t => t.Ignore())
                .ForMember(d => d.AssetCode, t => t.Ignore())
                .ForMember(d => d.IsDeleted, t => t.Ignore())
                .ForMember(d => d.Location, t => t.Ignore())
                .ForMember(d => d.Category, t => t.Ignore())
                .ForMember(d => d.State, t => t.Ignore());
        }

        private void FromDataAccessorLayer()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.FullName, t => t.MapFrom(src => src.FirstName + " " + src.LastName));
            CreateMap<User, AccountDto>()
                .ForMember(d => d.FullName, t => t.MapFrom(src => src.FirstName + " " + src.LastName))
                .ForMember(d => d.Token, t => t.Ignore());
            CreateMap<Category, CategoryDto>();
            CreateMap<State, StateDto>();
        }

        private void MapperAssetFromPresentationLayer()
        {
            CreateMap<Asset, AssetDto>()
                .ForMember(d => d.Category, t => t.MapFrom(c => c.Category.CategoryName))
                .ForMember(d => d.State, t => t.MapFrom(c => c.State.StateName));
        }
    }
}

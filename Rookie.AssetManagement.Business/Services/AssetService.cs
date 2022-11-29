﻿using AutoMapper;
using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Rookie.AssetManagement.Business.Interfaces;
using Rookie.AssetManagement.Contracts;
using Rookie.AssetManagement.Contracts.Dtos.AssetDtos;
using Rookie.AssetManagement.Contracts.Dtos.AssetDtos;
using Rookie.AssetManagement.Contracts.Dtos.UserDtos;
using Rookie.AssetManagement.DataAccessor.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rookie.AssetManagement.Business.Services
{
    public class AssetService : IAssetService
    {
        private readonly IBaseRepository<Asset> _assetRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<State> _stateRepository;
        private readonly IMapper _mapper;

        public AssetService(IBaseRepository<Asset> assetRepository, IBaseRepository<Category> categoryRepository,
            IBaseRepository<State> stateRepository, IMapper mapper)
        {
            _assetRepository = assetRepository;
            _categoryRepository = categoryRepository;
            _stateRepository = stateRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<AssetDto>> GetAllAsync()
        {
            var listAsset = _mapper.Map<IEnumerable<AssetDto>>(await _assetRepository.Entities.ToListAsync());
            return (List<AssetDto>)listAsset;
        }

        public async Task<PagedResponseModel<AssetDto>> GetByPageAsync(
            AssetQueryCriteriaDto assetQueryCriteria,
            CancellationToken cancellationToken, string location)
        {
            var assetQuery = AssetFilter(
              _assetRepository.Entities
              .Include(a => a.Category)
              .Include(a => a.State)
              .Where(x => !x.IsDeleted && x.Location == location).AsQueryable(),
              assetQueryCriteria);

            var asset = await assetQuery
               .AsNoTracking()
               .PaginateAsync<Asset>(
                   assetQueryCriteria.Page,
                   assetQueryCriteria.Limit,
                   cancellationToken);

            var assetDto = _mapper.Map<IEnumerable<AssetDto>>(asset.Items);

            return new PagedResponseModel<AssetDto>
            {
                CurrentPage = asset.CurrentPage,
                TotalPages = asset.TotalPages,
                TotalItems = asset.TotalItems,
                Items = assetDto
            };
        }
        public async Task<AssetDto> AddAssetAsync(AssetCreateDto asset, string location)
        {
            Ensure.Any.IsNotNull(asset);
            var newAsset = _mapper.Map<Asset>(asset);
            var getCategory = _categoryRepository.Entities.Where(x => x.Id == asset.Category).FirstOrDefault();
            if (getCategory == null)
            {
                throw new NotFoundException("Category Not Found!");
            }
            var getState = _stateRepository.Entities.Where(x => x.Id == asset.State).FirstOrDefault();
            if (getState == null)
            {
                throw new NotFoundException("State Not Found!");
            }
            newAsset.Category = getCategory;
            newAsset.State = getState;
            newAsset.Location = location;
            newAsset.IsDeleted = false;
            newAsset.AssetCode = GenerateAssetCode(newAsset);
            var createResult = await _assetRepository.Add(newAsset);
            return _mapper.Map<AssetDto>(newAsset);
        }
        private string GenerateAssetCode(Asset asset)
        {
            var assetCode = "";
            var code = "";
            var category = asset.Category.CategoryName.ToUpper();
            var lastAsset = _assetRepository.Entities.Where(x => x.Category == asset.Category)
                                .OrderByDescending(x => x.AssetCode).FirstOrDefault();
            if (lastAsset != null)
            {
                code = lastAsset.AssetCode.Substring(2);
            }
            switch (category)
            {
                case "LAPTOP":
                    assetCode = lastAsset == null ? "LA000001" : ("LA" + (Convert.ToInt32(code) + 1).ToString("D6"));
                    break;
                case "MONITOR":
                    assetCode = lastAsset == null ? "MO000001" : ("MO" + (Convert.ToInt32(code) + 1).ToString("D6"));
                    break;
                case "PERSONAL COMPUTER":
                    assetCode = lastAsset == null ? "PC000001" : ("PC" + (Convert.ToInt32(code) + 1).ToString("D6"));
                    break;
            }
            return assetCode;

        }
        private IQueryable<Asset> AssetFilter(
           IQueryable<Asset> assetQuery,
           AssetQueryCriteriaDto assetQueryCriteria)
        {
            if (!String.IsNullOrEmpty(assetQueryCriteria.Search))
            {
                assetQuery = assetQuery.Where(b =>
                  (b.AssetName.ToLower()).Contains(assetQueryCriteria.Search.ToLower())
                    || b.AssetCode.ToLower().Contains(assetQueryCriteria.Search.ToLower()));
            }

            if (assetQueryCriteria.Categories != null && !assetQueryCriteria.Categories.Any(e => e == "ALL"))
            {
                assetQuery = assetQuery.Where(c => assetQueryCriteria.Categories.Any(e => e == c.Category.Id.ToString()));
            }
            if (assetQueryCriteria.States != null && !assetQueryCriteria.States.Any(e => e == "ALL"))
            {
                assetQuery = assetQuery.Where(x => assetQueryCriteria.States.Any(e => e == x.State.Id.ToString()));
            }
            if (assetQueryCriteria.SortColumn != null)
            {
                var sortColumn = assetQueryCriteria.SortColumn.ToUpper();
                switch (sortColumn)
                {
                    case "ASSETCODE":
                        assetQuery = assetQueryCriteria.SortOrder == 0 ? assetQuery.OrderBy(x => x.AssetCode) : assetQuery.OrderByDescending(x => x.AssetCode);
                        break;
                    case "ASSETNAME":
                        assetQuery = assetQueryCriteria.SortOrder == 0 ? assetQuery.OrderBy(x => x.AssetName) : assetQuery.OrderByDescending(x => x.AssetName);
                        break;
                    case "CATEGORY":
                        assetQuery = assetQueryCriteria.SortOrder == 0 ? assetQuery.OrderBy(x => x.Category) : assetQuery.OrderByDescending(x => x.Category);
                        break;
                    case "STATE":
                        assetQuery = assetQueryCriteria.SortOrder == 0 ? assetQuery.OrderBy(x => x.State) : assetQuery.OrderByDescending(x => x.State);
                        break;
                    default:
                        assetQuery = assetQuery.OrderBy(x => x.AssetCode);
                        break;
                }

            }
            return assetQuery;
        }
    }
}
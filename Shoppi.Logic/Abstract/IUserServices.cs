﻿using Shoppi.Data.Models;
using System.Threading.Tasks;

namespace Shoppi.Logic.Abstract
{
    public interface IUserServices
    {
        Task SetDefaultAddressAsync(string userId, int addressId);

        Task<ShoppiUser> GetByIdAsync(string id);

        Task<ShoppiUser> GetByIdWithDefaultAddressAsync(string id);

        Task<int?> GetUsersDefaultAddressIdAsync(string id);
    }
}
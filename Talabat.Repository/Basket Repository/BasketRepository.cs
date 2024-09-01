using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Repository.Basket_Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        public async Task<CustmorBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustmorBasket>(basket);

        }

        public async Task<CustmorBasket?> UpdateBasketAsync(CustmorBasket basket)
        {
            var CreateOrUpdate = await _database.StringSetAsync(basket.Id , JsonSerializer.Serialize(basket) , TimeSpan.FromDays(30));
            if (!CreateOrUpdate) return null;
            return await GetBasketAsync(basket.Id);
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
    }
}

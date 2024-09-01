using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;

namespace Talabat.Core.Repositories.Contract
{
    public interface IBasketRepository
    {
        Task<CustmorBasket?> GetBasketAsync(string basketId);
        
        Task<CustmorBasket?> UpdateBasketAsync(CustmorBasket basket);

        Task<bool> DeleteBasketAsync(string basketId);
    }
}

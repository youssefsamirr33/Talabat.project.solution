using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;

namespace Talabat.Core.Repositories.Contract.Payment_Contract
{
    public interface IPaymentService
    {
        Task<CustmorBasket?> CreateOrUpdatePaymentIntent(string basketId);
    }
}

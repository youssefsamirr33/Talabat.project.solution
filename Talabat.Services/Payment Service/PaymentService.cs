using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Repositories.Contract.Payment_Contract;
using Product = Talabat.Core.Entities.Product;
using Talabat.Core.Entities.Order;

namespace Talabat.Services.Payment_Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration ,
                              IBasketRepository basketRepo,
                              IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustmorBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripesettings:SecretKey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0M;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod =await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingPrice = shippingPrice;
            }

            if(basket.Items?.Count > 0)
            {
                
                var productRepo = _unitOfWork.Repository<Product>();
                foreach(var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create new paymwnt Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(I => I.Price * 100 * I.Quantity) + (long)(shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;

            }
            else // Update Exist payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(I => I.Price * 100 * I.Quantity) + (long)(shippingPrice * 100)
                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId , options);
            }

            await _basketRepo.UpdateBasketAsync(basket);

            return basket;

        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Repositories.Contract.Order_contract;
using Talabat.Core.Repositories.Contract.Payment_Contract;
using Talabat.Core.Specifications.Order_Spec;

namespace Talabat.Services.Order.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo ,
                            IUnitOfWork unitOfWork,
                            IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Core.Entities.Order.Order?> CreateOrderAsync(string buyerEmail, OrderAddress shippingAddress , string basketId, int deliveryMethodId)
        {
            // 1.Get Basket From Baskets Repo
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id); 
                    var productOrderItem = new OrderProduct(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrderItem , product.Price , item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);



            var OrderRepo = _unitOfWork.Repository<Core.Entities.Order.Order>();

            var spec = new OrderWithPaymentIntentSpecification(basket?.PaymentIntentId ?? "");
            var orderExist = await _unitOfWork.Repository<Core.Entities.Order.Order>().GetWithSpecAsync(spec);

            if(orderExist is not null)
            {
                OrderRepo.Delete(orderExist);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            // 5. Create Order

            var order = new Core.Entities.Order.Order(
                buyerEmail: buyerEmail,
                shippingAddress: shippingAddress,
                items: orderItems,
                deliveryMethod: deliveryMethod,
                subTotal: subtotal,
                paymentIntentId : basket?.PaymentIntentId ?? ""
                );

            OrderRepo.Add(order);

            // 6. Save To Database [TODO]
            var count = await _unitOfWork.CompleteAsync();

            if (count <= 0) return null;
            return order;

        }


        public Task<IReadOnlyList<Core.Entities.Order.Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders = _unitOfWork.Repository<Core.Entities.Order.Order>().GetAllWithSpecAsync(spec);
            //if (orders is null) return null;
            return orders;
        }
        public async Task<Core.Entities.Order.Order?> GetOrderByIdForUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecifications(buyerEmail, OrderId);
            var order = await _unitOfWork.Repository<Core.Entities.Order.Order>().GetWithSpecAsync(spec);
            if (order is null) return null;
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod>().GetAsync();

     


    }
}

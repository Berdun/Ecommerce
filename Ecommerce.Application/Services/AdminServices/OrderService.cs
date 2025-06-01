using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AdminServices
{
    public class OrderService
    {
        private readonly IUnitOfWork _uow;

        public OrderVM OrderVM { get; set; }

        public OrderService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<OrderProduct> GetAll()
        {
            var orderList = _uow.OrderProduct.GetAll(x => x.OrderStatus != "Delivered");
            return orderList;
        }

        public OrderVM Details(int id)
        {
            OrderVM = new OrderVM
            {
                OrderProduct = _uow.OrderProduct.GetFirstOrDefault(
                    o => o.Id == id, includeProperties: "AppUser"),
                OrderDetails = _uow.OrderDetails.GetAll(
                    d => d.OrderProductId == id, includeProperties: "Product")
            };
            return OrderVM;
        }

        public OrderVM Delivered(OrderVM orderVM)
        {
            var orderProduct = _uow.OrderProduct.GetFirstOrDefault(
                op => op.Id == orderVM.OrderProduct.Id);

            orderProduct.OrderStatus = "Delivered";

            _uow.OrderProduct.Update(orderProduct);
            _uow.Save();

            return orderVM;
        }

        public OrderVM CancelOrder(OrderVM orderVM)
        {
            var orderProduct = _uow.OrderProduct.GetFirstOrDefault(
                op => op.Id == orderVM.OrderProduct.Id);

            orderProduct.OrderStatus = "Cancel";

            _uow.OrderProduct.Update(orderProduct);
            _uow.Save();

            return orderVM;
        }

        public OrderVM UpdateOrderDetails(OrderVM orderVM)
        {
            var orderProduct = _uow.OrderProduct.GetFirstOrDefault(op => op.Id == orderVM.OrderProduct.Id);

            orderProduct.Address = orderVM.OrderProduct.Address;
            orderProduct.PostalCode = orderVM.OrderProduct.PostalCode;
            orderProduct.CellPhone = orderVM.OrderProduct.CellPhone;
            orderProduct.Name = orderVM.OrderProduct.Name;

            _uow.OrderProduct.Update(orderProduct);
            _uow.Save();

            return orderVM;
        }



    }
}

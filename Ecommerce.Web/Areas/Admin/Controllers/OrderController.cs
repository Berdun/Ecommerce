using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;
        public OrderVM OrderVM { get; set; }

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View(_orderService.GetAll());
        }


        public IActionResult Details(int Id)
        {
            return View(_orderService.Details(Id));
        }

        [HttpPost]
        public IActionResult Delivered(OrderVM orderVM)
        {
            var order = _orderService.Delivered(orderVM);

            return RedirectToAction("Details", "Order", new { Id = order.OrderProduct.Id });
        }

        [HttpPost]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var order = _orderService.CancelOrder(orderVM);

            return RedirectToAction("Details", "Order", new { Id = order.OrderProduct.Id });
        }

        [HttpPost]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {
            var order = _orderService.UpdateOrderDetails(orderVM);

            return RedirectToAction("Details", "Order", new { Id = order.OrderProduct.Id });
        }

    }
}

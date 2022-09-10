using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using B_Gallery.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace B_Gallery.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string? status)
        {
            GetAll(status);
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string? status)
        {
            IEnumerable<OrderHeader> orderHeaders;
            if (status != "all")
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus.ToLower() == status, includePropertis: "ApplicationUser");
                return Json(new { data = orderHeaders });
            }
            orderHeaders = _unitOfWork.OrderHeader.GetAll(includePropertis: "ApplicationUser");
            return Json(new { data = orderHeaders });
        }
    }
}

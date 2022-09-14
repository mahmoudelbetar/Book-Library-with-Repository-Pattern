using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using B_Gallery.Models.ViewModels;
using B_Gallery.Utility;
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
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            switch (status)
            {
                case "pending":
                    orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == SD.PaymentStatusPending, includePropertis: "ApplicationUser");
                    break;
                case "inprocess":
                    orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == SD.StatusInProcess, includePropertis: "ApplicationUser");
                    break;
                case "completed":
                    orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.OrderStatus == SD.StatusShipped, includePropertis: "ApplicationUser");
                    break;
                default:
                    orderHeaders = _unitOfWork.OrderHeader.GetAll(includePropertis: "ApplicationUser");
                    break;
            }

            return Json(new { data = orderHeaders });

        }
    }
}

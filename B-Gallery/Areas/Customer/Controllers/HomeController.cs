using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using B_Gallery.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace B_Gallery.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includePropertis:"Category,CoverType");
            return View(products);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cartObj = new ShoppingCart()
            {
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id, includePropertis: "Category,CoverType")
            };
            return View(cartObj);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
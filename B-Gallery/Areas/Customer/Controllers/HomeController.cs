using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using B_Gallery.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace B_Gallery.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _http;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IHttpContextAccessor Http)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _http = Http;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includePropertis: "Category");
            HttpContext.Session.SetInt32("ccart", _unitOfWork.ShoppingCart.CountCart());
            return View(products);

        }

        [HttpGet]
        public IActionResult Filter(string? category = null)
        {
            if (category != null)
            {
                if (category != "All")
                {
                    IEnumerable<Product> filteredProdcts = _unitOfWork.Product.FilterProductByCategory(category);
                    HttpContext.Session.SetInt32("ccart", _unitOfWork.ShoppingCart.CountCart());
                    return View("Index", filteredProdcts);
                }
                else
                {
                    IEnumerable<Product> products = _unitOfWork.Product.GetAll(includePropertis: "Category");
                    HttpContext.Session.SetInt32("ccart", _unitOfWork.ShoppingCart.CountCart());
                    return View("Index", products);
                }
            }
            else
            {
                IEnumerable<Product> products = _unitOfWork.Product.GetAll(includePropertis: "Category");
                HttpContext.Session.SetInt32("ccart", _unitOfWork.ShoppingCart.CountCart());
                return View("Index", products);
            }
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cartObj = new ShoppingCart()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == productId, includePropertis: "Category")
            };
            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)User.Identity;
                Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ProductId == shoppingCart.ProductId && u.ApplicationUserId == claim.Value);
                if(cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                }
                else
                {
                    cartFromDb.Count = shoppingCart.Count;
                }
                var shoppingCartViewModel = new ShoppingCartViewModel()
                {
                    ListCart = _unitOfWork.ShoppingCart.GetAll()
                };
                HttpContext.Session.SetInt32("ccart", shoppingCartViewModel.ListCart.Sum(c => c.Count));
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(shoppingCart);
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
using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using B_Gallery.Models.ViewModels;
using B_Gallery.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace B_Gallery.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Individual")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)User.Identity;
            Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };
            var total = shoppingCartViewModel.OrderHeader.OrderTotal;
            foreach(var item in shoppingCartViewModel.ListCart)
            {
                item.Product.ListPrice = item.Count * item.Product.Price;
                total = (double)shoppingCartViewModel.ListCart.Sum(s => s.Product.ListPrice);
            }
            shoppingCartViewModel.OrderHeader.OrderTotal = total;
            HttpContext.Session.SetInt32("ccart", _unitOfWork.ShoppingCart.CountCart());
            return View(shoppingCartViewModel);
        }

        [HttpGet]
        public IActionResult Plus(string cartId)
        {
            var item = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            item.Count += 1;
            _unitOfWork.ShoppingCart.Update(item);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Minus(string cartId)
        {
            var item = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if(item.Count <= 0)
            {
                item.Count = 0;
            }
            else
            {
                item.Count -= 1;
            }
            _unitOfWork.ShoppingCart.Update(item);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(string cartId)
        {
            var shoppingCartItem = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(shoppingCartItem);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Summary()
        {
            ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)User.Identity;
            Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product"),
                OrderHeader = new()
            };


            var userDetails = shoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == claim.Value);

            shoppingCartViewModel.OrderHeader.PhoneNumber = userDetails.PhoneNumber;
            shoppingCartViewModel.OrderHeader.StreetAddress = userDetails.StreetAddress;
            shoppingCartViewModel.OrderHeader.City = userDetails.City;
            shoppingCartViewModel.OrderHeader.State = userDetails.State;
            shoppingCartViewModel.OrderHeader.PostalCode = userDetails.postalCode;
            shoppingCartViewModel.OrderHeader.Name = userDetails.Name;

            var total = shoppingCartViewModel.OrderHeader.OrderTotal;
            foreach (var item in shoppingCartViewModel.ListCart)
            {
                item.Product.ListPrice = item.Count * item.Product.Price;
                total = (double)shoppingCartViewModel.ListCart.Sum(s => s.Product.ListPrice);
            }
            shoppingCartViewModel.OrderHeader.OrderTotal = total;
            
            return View(shoppingCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPOST(ShoppingCartViewModel shoppingCartViewModel)
        {
            ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)User.Identity;
            Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartViewModel.ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product");
            shoppingCartViewModel.OrderHeader = new()
            {
                ApplicationUserId = claim.Value,
                PaymentStatus = SD.PaymentStatusPending,
                OrderStatus = SD.StatusPending,
                OrderDate = DateTime.Now,
                City = shoppingCartViewModel.OrderHeader.City,
                State = shoppingCartViewModel.OrderHeader.State,
                PostalCode = shoppingCartViewModel.OrderHeader.PostalCode,
                Name = shoppingCartViewModel.OrderHeader.Name,
                PhoneNumber = shoppingCartViewModel.OrderHeader.PhoneNumber,
                StreetAddress = shoppingCartViewModel.OrderHeader.StreetAddress
            };

            var total = shoppingCartViewModel.OrderHeader.OrderTotal;
            foreach (var item in shoppingCartViewModel.ListCart)
            {
                item.Product.ListPrice = item.Count * item.Product.Price;
                total = (double)shoppingCartViewModel.ListCart.Sum(s => s.Product.ListPrice);
            }
            shoppingCartViewModel.OrderHeader.OrderTotal = total;

            _unitOfWork.OrderHeader.Add(shoppingCartViewModel.OrderHeader);
            _unitOfWork.Save();

            foreach(var cart in shoppingCartViewModel.ListCart)
            {
                OrderDetail orderDetail = new()
                {
                    Count = cart.Count,
                    Price = cart.Product.Price,
                    ProductId = cart.Product.Id,
                    OrderId = shoppingCartViewModel.OrderHeader.Id
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            _unitOfWork.ShoppingCart.RemoveRange(shoppingCartViewModel.ListCart);
            _unitOfWork.Save();
            TempData["OrderSuccess"] = "Order Placed Successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}

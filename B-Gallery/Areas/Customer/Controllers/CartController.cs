using B_Gallery.DataAccess.Repository.IRepository;
using B_Gallery.Models;
using B_Gallery.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace B_Gallery.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
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
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            var total = shoppingCartViewModel.Total;
            foreach(var item in shoppingCartViewModel.ListCart)
            {
                item.Product.ListPrice = item.Count * item.Product.Price;
                total = (decimal)shoppingCartViewModel.ListCart.Sum(s => s.Product.ListPrice);
            }
            shoppingCartViewModel.Total = total;
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

    }
}

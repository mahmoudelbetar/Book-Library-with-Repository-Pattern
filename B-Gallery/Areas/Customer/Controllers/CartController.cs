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
            foreach(var item in shoppingCartViewModel.ListCart)
            {
                item.Product.ListPrice = item.Count * item.Product.Price;
                item.Product.Price50 = shoppingCartViewModel.ListCart.Sum(s => s.Product.ListPrice);
            }
            return View(shoppingCartViewModel);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var item = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == id.ToString());
            if(item == null)
            {
                return NotFound();
            }
            _unitOfWork.ShoppingCart.Remove(item);
            _unitOfWork.Save();
            ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)User.Identity;
            Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartViewModel shoppingCartViewModel = new ShoppingCartViewModel()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            
            return View("Index", shoppingCartViewModel);
        }

        //[HttpGet]
        //public IActionResult Increase(int id)
        //{
        //    ClaimsIdentity? claimsIdentity = (ClaimsIdentity?)User.Identity;
        //    Claim? claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        //    ShoppingCart shoppingCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(u => u.ProductId == id);

        //}
    }
}

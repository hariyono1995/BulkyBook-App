using FinalBulkyBook.DataAccess.Repository.IRepository;
using FinalBulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinalBulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnityOfWork _unityOfWork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public int OrderTotal { get; set; }

        public CartController(IUnityOfWork unityOfWork)
        {
            _unityOfWork = unityOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                ListCarts = _unityOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };

            foreach(var cart in ShoppingCartVM.ListCarts)
            {
                cart.Price = GetPriceOnBasedQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        public IActionResult Summary()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //ShoppingCartVM = new ShoppingCartVM()
            //{
            //    ListCarts = _unityOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            //};

            //foreach (var cart in ShoppingCartVM.ListCarts)
            //{
            //    cart.Price = GetPriceOnBasedQuantity(cart.Count, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
            //    ShoppingCartVM.CartTotal += (cart.Price * cart.Count);
            //}

            //return View(ShoppingCartVM);
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var cartItem = _unityOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

            _unityOfWork.ShoppingCart.IncrementCount(cartItem, 1);
            _unityOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartItem = _unityOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);
            if(cartItem.Count <= 1)
            {
                _unityOfWork.ShoppingCart.Remove(cartItem);
            }
            else
            {
                _unityOfWork.ShoppingCart.DecrementCount(cartItem, 1);
            }
            _unityOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cartItem = _unityOfWork.ShoppingCart.GetFirstOrDefault(u => u.Id == cartId);

           if(cartItem != null)
            {
                _unityOfWork.ShoppingCart.Remove(cartItem);
            }
            _unityOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceOnBasedQuantity (double quantity, double price, double price50, double price100)
        {
            if(quantity <= 50)
                return price;
            else
            {
                if (quantity <= 100)
                    return price50;
                return price100;
            }
        }
    }
}

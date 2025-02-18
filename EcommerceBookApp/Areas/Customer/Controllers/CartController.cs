using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;
using EcommerceBookApp.Models.ViewModels;
using EcommerceBookApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceBookApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty] //To Bind the properties explicitly
        public ShoppinCartVM ShoppinCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppinCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader=new()
            };
            foreach (var cart in ShoppinCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppinCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppinCartVM);
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.GetFirstOrDefault(u => u.Id == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppinCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };
            ShoppinCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == userId);
            ShoppinCartVM.OrderHeader.Name = ShoppinCartVM.OrderHeader.ApplicationUser.Name;
            ShoppinCartVM.OrderHeader.PhoneNumber = ShoppinCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppinCartVM.OrderHeader.StreetAddress = ShoppinCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppinCartVM.OrderHeader.City = ShoppinCartVM.OrderHeader.ApplicationUser.City;
            ShoppinCartVM.OrderHeader.State = ShoppinCartVM.OrderHeader.ApplicationUser.state;
            ShoppinCartVM.OrderHeader.PostalCode = ShoppinCartVM.OrderHeader.ApplicationUser.PostalCode;
            foreach (var cart in ShoppinCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppinCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppinCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
		public IActionResult SummaryPOST()
		{

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
			ShoppinCartVM.ShoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");
            ShoppinCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppinCartVM.OrderHeader.ApplicationUserId = userId;
			ShoppinCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(u => u.Id == userId);
            foreach (var cart in ShoppinCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppinCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			if (ShoppinCartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault() == 0)
			{
                //It is a regular customer account we need to capture the payment
                ShoppinCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                ShoppinCartVM.OrderHeader.OrderStatus = SD.StatusPending;
             }
            else
            {
				//It is a company user
				ShoppinCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				ShoppinCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			}
            _unitOfWork.OrderHeaderRepository.Add(ShoppinCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach(var cart in ShoppinCartVM.ShoppingCartList)
            {
                OrderDetail OrderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppinCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Quantity = cart.Count
                };
                _unitOfWork.OrderDetailRepository.Add(OrderDetail);
                _unitOfWork.Save();
            }
			return View(ShoppinCartVM);
		}

		private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
    }
}

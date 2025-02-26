using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;
using EcommerceBookApp.Utility;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Diagnostics;

namespace EcommerceBookApp.Areas.Admin.Controllers
{
	[Area("admin")]
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

		#region API CALLS
		[HttpGet]
		public IActionResult GetALL(string status)
		{
			List<OrderHeader> orderHeaders = _unitOfWork.OrderHeaderRepository.GetAll(includeProperties: "ApplicationUser").ToList();
			switch (status)
			{
				case "inprocess":
					orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess).ToList();
					break;
				case "pending":
					orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.StatusPending).ToList();
					break;
				case "completed":
					orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.StatusInShipped).ToList();
					break;
				case "approved":
					orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.StatusApproved).ToList();
					break;
				default:
					break;
			}
			return Json(new { data = orderHeaders });
		}
		#endregion
	}
}

using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;
using EcommerceBookApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommerceBookApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> dbProductList = _unitOfWork.ProductRepository.GetAll().ToList();
			return View(dbProductList);
        }

		public IActionResult Create()
		{
			//Using EF Core Projections to convert form IEnumerable<Category> to IEnumerable<SelectListItem>
			IEnumerable<SelectListItem> categoryList = _unitOfWork.CategoryRepository.GetAll().
				Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
			//ViewBag.CategoryList = categoryList;
			//ViewData["CategoryList"] = categoryList;
			ProductVM productVM = new()
			{
				CategoryList = categoryList,
				Product = new Product()
			};
			return View(productVM);
		}

		[HttpPost]
		public IActionResult Create(ProductVM productVM)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.ProductRepository.Add(productVM.Product); //Used to persist the Category dataobject in to the table
				_unitOfWork.Save();
				TempData["success"] = "Product created Sucessfully!!";
				return RedirectToAction("Index", "Product"); // Redirects to the Index in Category
			}
			else
			{
				productVM.CategoryList = _unitOfWork.CategoryRepository.GetAll().
				Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
				return View(productVM);
			}
			
		}

		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product? productFromDb = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id); //Find only works on the primary key 
																										  
			if (productFromDb == null)
			{
				return NotFound();
			}
			return View(productFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Product product)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.ProductRepository.Update(product); //Used to update an existing record
				_unitOfWork.Save();
				TempData["success"] = "Product updated Sucessfully!!";
				return RedirectToAction("Index", "Product"); // Redirects to the Index in Product
			}
			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Product? productFromDb = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id); //Find only works on the primary key 
																										
			if (productFromDb == null)
			{
				return NotFound();
			}
			return View(productFromDb);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Product? productObj = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);
			if (productObj == null)
			{
				return NotFound();
			}
			_unitOfWork.ProductRepository.Remove(productObj);
			_unitOfWork.Save();
			TempData["success"] = "Product deleted Sucessfully!!";
			return RedirectToAction("Index", "Product");
		}

	}
}

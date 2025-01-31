using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;
using EcommerceBookApp.Models.ViewModels;
using EcommerceBookApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Packaging.Signing;

namespace EcommerceBookApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ROLE_ADMIN)] //Only allows Admin users to access it.
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> dbProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
			return View(dbProductList);
        }

		public IActionResult Upsert(int? id)
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
			if(id==null || id==0)
			{
				//For create
				return View(productVM);
			}
			else
			{
				productVM.Product = _unitOfWork.ProductRepository.GetFirstOrDefault(u => u.Id == id);
				return View(productVM);
			}	
			
		}

		[HttpPost]
		public IActionResult Upsert(ProductVM productVM,IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				if(file !=null)
				{
					string fileName= Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
					string productPath = Path.Combine(wwwRootPath,@"images\product");
					if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
					{
						//Delete the old image
						var oldImagePath= Path.Combine(wwwRootPath,productVM.Product.ImageUrl.TrimStart('\\'));
						if(System.IO.File.Exists(oldImagePath))
						{
							System.IO.File.Delete(oldImagePath);
						}
					}
					
					using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
					{
						file.CopyTo(fileStream); //Copies the file to the desired location
					}
					productVM.Product.ImageUrl = @"\images\product\" + fileName;

                }
				if(productVM.Product.Id ==0)
				{
					_unitOfWork.ProductRepository.Add(productVM.Product); //Used to persist the Category dataobject in to the table
				}
				else
				{
					_unitOfWork.ProductRepository.Update(productVM.Product);
				}

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

		#region API CALLS
		[HttpGet]
		public IActionResult GetaLL()
		{
            List<Product> dbProductList = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new {data=dbProductList});
        }
        #endregion
    }
}

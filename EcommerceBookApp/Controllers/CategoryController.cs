using EcommerceBookApp.Data;
using EcommerceBookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBookApp.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public CategoryController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            //Retrieving the category list from SQL Category table.
            List<Category> objCategoryList = _applicationDbContext.Categories.ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(category.Name !=null && category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The display order cannot exactly match the name"); //Name is the key
            }
           
			if (ModelState.IsValid)
            {
				_applicationDbContext.Categories.Add(category); //Used to persist the Category dataobject in to the table
				_applicationDbContext.SaveChanges();
				TempData["success"] = "Category created Sucessfully!!";
				return RedirectToAction("Index", "Category"); // Redirects to the Index in Category
			}
            return View();
        }

		public IActionResult Edit(int? id)
		{
            if(id == null || id==0) {
                return NotFound();
            }
            Category? categoryFromDb = _applicationDbContext.Categories.Find(id); //Find only works on the primary key 
			//Category? categoryFromDb1 = _applicationDbContext.Categories.FirstOrDefault(u => u.Id == id);//Uses LINQ
			if (categoryFromDb == null)
            {
                return NotFound();
            }
			return View(categoryFromDb);
		}

		[HttpPost]
		public IActionResult Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				_applicationDbContext.Categories.Update(category); //Used to update an existing record
				_applicationDbContext.SaveChanges();
				TempData["success"] = "Category updated Sucessfully!!";
				return RedirectToAction("Index", "Category"); // Redirects to the Index in Category
			}
			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? categoryFromDb = _applicationDbContext.Categories.Find(id); //Find only works on the primary key 
																				  //Category? categoryFromDb1 = _applicationDbContext.Categories.FirstOrDefault(u => u.Id == id);//Uses LINQ
			if (categoryFromDb == null)
			{
				return NotFound();
			}
			return View(categoryFromDb);
		}

		[HttpPost,ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? categoryObj= _applicationDbContext.Categories.Find(id);
			if(categoryObj==null)
			{
				return NotFound();
			}
			_applicationDbContext.Categories.Remove(categoryObj);
			_applicationDbContext.SaveChanges();
			TempData["success"] = "Category deleted Sucessfully!!";
			return RedirectToAction("Index","Category");
		}
	}
}

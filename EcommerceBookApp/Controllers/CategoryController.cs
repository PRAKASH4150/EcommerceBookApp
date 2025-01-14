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
            _applicationDbContext.Categories.Add(category); //Used to persist the Category dataobject in to the table
            _applicationDbContext.SaveChanges();
            return RedirectToAction("Index","Category"); // Redirects to the Index in Category
        }
    }
}

using EcommerceBookApp.DataAccess.Repository;
using EcommerceBookApp.DataAccess.Repository.IRepository;
using EcommerceBookApp.Models;
using EcommerceBookApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBookApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.ROLE_ADMIN)] //Only allows Admin users to access it.
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //Retrieving the category list from SQL Category table.
            List<Category> objCategoryList = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name != null && category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The display order cannot exactly match the name"); //Name is the key
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category); //Used to persist the Category dataobject in to the table
                _unitOfWork.Save();
                TempData["success"] = "Category created Sucessfully!!";
                return RedirectToAction("Index", "Category"); // Redirects to the Index in Category
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id); //Find only works on the primary key 
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
                _unitOfWork.CategoryRepository.Update(category); //Used to update an existing record
                _unitOfWork.Save();
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
            Category? categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id); //Find only works on the primary key 
                                                                                                          //Category? categoryFromDb1 = _applicationDbContext.Categories.FirstOrDefault(u => u.Id == id);//Uses LINQ
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? categoryObj = _unitOfWork.CategoryRepository.GetFirstOrDefault(u => u.Id == id);
            if (categoryObj == null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Remove(categoryObj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted Sucessfully!!";
            return RedirectToAction("Index", "Category");
        }
    }
}

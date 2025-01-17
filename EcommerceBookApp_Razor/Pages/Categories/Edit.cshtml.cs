using EcommerceBookApp_Razor.Data;
using EcommerceBookApp_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceBookApp_Razor.Pages.Categories
{
    public class EditModel : PageModel
    {
		[BindProperty]
		public Category? Category { get; set; }
		private readonly ApplicationDbContext _applicationDbContext;
        public EditModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void OnGet(int? id)
        {
           if(id!=null)
            {
				Category= _applicationDbContext.Categories.Find(id);
            }
            
        }
        public IActionResult OnPost(Category category)
        {
			if (ModelState.IsValid)
			{
				_applicationDbContext.Categories.Update(category); //Used to update an existing record
				_applicationDbContext.SaveChanges();
				TempData["success"] = "Category updated Sucessfully!!";
			}
			return RedirectToPage("Index"); // Redirects to the Index in Category
		}
    }
}

using EcommerceBookApp_Razor.Data;
using EcommerceBookApp_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceBookApp_Razor.Pages.Categories
{
	//[BindProperties] Applicable to class level.
	public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _applicationDbContext;
        [BindProperty] //Applicable only to razor pages. If kept above the class binds all the properties in the class.
        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext applicationDbContext)
        {
			_applicationDbContext = applicationDbContext;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost(Category category)
        {
            _applicationDbContext.Categories.Add(category);
            _applicationDbContext.SaveChanges();
            TempData["success"] = "Category created Sucessfully!!";
            return RedirectToPage("Index");
        }
    }
}

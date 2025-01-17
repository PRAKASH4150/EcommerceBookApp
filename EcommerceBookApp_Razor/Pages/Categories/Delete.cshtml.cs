using EcommerceBookApp_Razor.Data;
using EcommerceBookApp_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceBookApp_Razor.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public Category? Category { get; set; }
        private readonly ApplicationDbContext _applicationDbContext;
        public DeleteModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void OnGet(int? id)
        {
            if(id!=null)
            {
                Category=_applicationDbContext.Categories.Find(id);
            }
        }

        public IActionResult OnPost(int id)
        {
            Category? categoryObj = _applicationDbContext.Categories.Find(id);
            if(categoryObj!=null)
            {
                _applicationDbContext.Categories.Remove(categoryObj);
                _applicationDbContext.SaveChanges();
                TempData["success"] = "Category deleted Sucessfully!!";
            }
			return RedirectToPage("Index");
		}
    }
}

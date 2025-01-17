using EcommerceBookApp_Razor.Data;
using EcommerceBookApp_Razor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EcommerceBookApp_Razor.Pages.Categories
{
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _applicationDbContext;
        public List<Category> CategoryList { get; set; }
        public IndexModel(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public void OnGet()
        {
            CategoryList = _applicationDbContext.Categories.ToList();
        }
    }
}

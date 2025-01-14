using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EcommerceBookApp.Models
{
    public class Category
    {
        //[Key] - Not required if the name of the variable is Id
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")] //This is the name which will be displayed on the UI.
        public string Name { get; set; }
        [DisplayName("Category Order")]
        public int DisplayOrder { get; set; }
    }
}

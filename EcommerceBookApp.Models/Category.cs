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
        [MaxLength(30)]
        [DisplayName("Category Name")] //This is the name which will be displayed on the UI.
        public string Name { get; set; }
        [DisplayName("Category Order")]
        [Range(1,100,ErrorMessage="Display order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}

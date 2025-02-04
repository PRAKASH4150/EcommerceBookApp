using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EcommerceBookApp.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Company Name")] //This is the name which will be displayed on the UI.
        public string Name { get; set; }
		[DisplayName("Street Address")]
		public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

		[DisplayName("Postal Code")]
		public string? PostalCode { get; set; }

		[DisplayName("Phone Number")]
		public string? PhoneNumber { get; set; }
    }
}

namespace EcommerceBookApp.Models.ViewModels
{
    public class ShoppinCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }
        public double OrderTotal { get; set; }
    }
}

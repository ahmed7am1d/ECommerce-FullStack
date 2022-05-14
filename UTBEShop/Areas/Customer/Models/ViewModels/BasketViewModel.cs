using UTBEShop.Models.Entities;


namespace UTBEShop.Areas.Customer.Models.ViewModels
{
    public class BasketViewModel
    {
        public List<OrderItem> Items { get; set; }
        public double TotalPrice { get; set; }
    }
}

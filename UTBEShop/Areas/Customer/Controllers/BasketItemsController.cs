using Microsoft.AspNetCore.Mvc;
using UTBEShop.Areas.Customer.Models.ViewModels;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Extensions;

namespace UTBEShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class BasketItemsController : Controller
    {
   
        [HttpPost]
        public IActionResult CustomerBasket()
            {
            BasketViewModel basketViewModel = new BasketViewModel();
            string orderItemsString = "OrderItems";
            //const string totalPriceString = "TotalPrice";
            double totalPrice = 0;
            //1- get all the orderItems 
            //Remeber => the list contains Orders each orderitem linked to main order with the specified amount 
            basketViewModel.Items = HttpContext.Session.GetObject<List<OrderItem>>(orderItemsString);
            if(basketViewModel.Items != null)
            {
                foreach (OrderItem item in basketViewModel.Items)
                {
                    totalPrice = totalPrice + item.Price;
                }
            }
            else
            {
                totalPrice = 0;
            }
            
            basketViewModel.TotalPrice = totalPrice;
            return PartialView(basketViewModel);
        }
    }
}

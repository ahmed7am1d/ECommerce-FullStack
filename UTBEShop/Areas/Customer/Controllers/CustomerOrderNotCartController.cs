using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UTBEShop.Controllers;
using UTBEShop.Models.ApplicationServices.Abstraction;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Entities.Identity;
using UTBEShop.Models.Extensions;
using UTBEShop.Models.Infrastructure.Database;

namespace UTBEShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = nameof(Roles.Customer))]
    public class CustomerOrderNotCartController : Controller
    {

        const string totalPriceString = "TotalPrice";
        const string orderItemsString = "OrderItems";

        #region Constructor and DI

        ISecurityApplicationService _iSecure;
        EShopDbContext _eShopDbContext;


        public CustomerOrderNotCartController(ISecurityApplicationService securityApplicationService, EShopDbContext eShopDb)
        {
            _iSecure = securityApplicationService;
            _eShopDbContext = eShopDb;
        }
        #endregion

        #region Method to add Items to session 
        //this method if everything successful will retrurn total Price after adding on item to session 
        //we added httpost so that this method can be called from the url as HttpRequest type POST
        /*
            JS Method using ajax: 
            function Buy(productId, urlAction, outElementId, locale) {
        $.ajax({
        type: "POST",
        url: urlAction,
        data: { productId: productId },
        dataType: "text",
        success: function (totalPrice) {
            ChangeTotalPriceInformation(outElementId, locale, totalPrice);
        },
        error: function (req, status, error) {
            $(outElementId).text('error during buying!');
        }

    });
}
         
         */

        [HttpPost]
        public double AddOrderItemsToSession(int? productId)
        {
            double totalPrice = 0;
           

            //-- Getting the Product with the choosen id 
            Product product = _eShopDbContext.Products.Where(prod => prod.Id == productId).FirstOrDefault();

            //--if the product is not null create new order for the user 
            if (product != null)
            {
                OrderItem orderItem = new OrderItem()
                {
                    ProductID = product.Id,
                    product = product,
                    Amount = 1,
                    Price = product.ProductPrice
                };

                //check again if the session still available - then create list of orderItems that will be bringed from current session
                if(HttpContext.Session.IsAvailable)
                {
                    List<OrderItem> orderItems = HttpContext.Session.GetObject<List<OrderItem>>(orderItemsString);
                    OrderItem orderItemInSession = null;
                    if (orderItems != null)
                        orderItemInSession = orderItems.Find(oi => oi.ProductID == orderItem.ProductID);
                    else
                        orderItems = new List<OrderItem>();

                    if(orderItemInSession != null)
                    {
                        ++orderItemInSession.Amount;
                        orderItemInSession.Price += orderItem.product.ProductPrice;
                    }
                    else
                    {
                        orderItems.Add(orderItem);
                    }

                    HttpContext.Session.SetObject(orderItemsString, orderItems);
                    foreach(OrderItem item in orderItems)
                    {
                        totalPrice = totalPrice + item.Price; 
                    }
                }


            }
            return totalPrice;

        }
        #endregion

        #region Method to approve  order in session
        public async Task<IActionResult> ApproveOrderInSession()
        {
            if(HttpContext.Session.IsAvailable)
            {
                double totalPrice = 0;
                List<OrderItem> orderItems = HttpContext.Session.GetObject<List<OrderItem>>(orderItemsString);
                if(orderItems != null)
                {
                    foreach(OrderItem orderItem in orderItems)
                    {
                        totalPrice += orderItem.product.ProductPrice * orderItem.Amount;
                        orderItem.product = null;
                    }


                    User currentUser = await _iSecure.GetCurrentUser(User);
                    Order order = new Order()
                    {
                        OrderNumber = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
                        TotalPrice = totalPrice,
                        OrderItems = orderItems,
                        UserID = currentUser.Id

                    };

                    //we can add just the order; order items will be added automatically 
                    await _eShopDbContext.AddAsync(order);
                    await _eShopDbContext.SaveChangesAsync();

                    HttpContext.Session.Remove(orderItemsString);
                    HttpContext.Session.Remove(totalPriceString);

                    return RedirectToAction(nameof(CustomerOrdersController.Index), nameof(CustomerOrdersController).Replace("Controller", ""), new { Area = nameof(Customer) });
                }
            }
            return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""), new { Area = "" });
        }
        #endregion
    }
}

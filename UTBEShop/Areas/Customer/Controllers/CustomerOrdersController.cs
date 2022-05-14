using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UTBEShop.Models.ApplicationServices.Abstraction;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Entities.Identity;
using UTBEShop.Models.Infrastructure.Database;

namespace UTBEShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = nameof(Roles.Customer))]
    public class CustomerOrdersController : Controller
    {

        ISecurityApplicationService _iSecure;
        EShopDbContext _eshopDbContext;

        public CustomerOrdersController(EShopDbContext eShopDbContext, ISecurityApplicationService securityApplicationService)
        {
            _iSecure = securityApplicationService;
            _eshopDbContext = eShopDbContext; ;
        }


        /// <summary>
        ///we want to show all the orders of the user 
        ///if user is authenticated we grab him and we try to get all his orders from the database and retunr them as list 
        /// </summary>
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                User currentUser = await _iSecure.GetCurrentUser(User);
                if (currentUser != null)
                {
                    IList<Order> userOrders = await _eshopDbContext.Orders
                        .Where(or => or.UserID == currentUser.Id)
                        .Include(o => o.user)
                        .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.product)
                        .ToListAsync();

                    return View(userOrders);
                }
         
            }
            return NotFound();
        }
    }
}

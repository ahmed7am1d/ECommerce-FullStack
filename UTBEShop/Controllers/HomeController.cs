using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UTBEShop.Models;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Infrastructure.Database;
namespace UTBEShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly EShopDbContext _eShopeDbContext;

        public HomeController(ILogger<HomeController> logger, EShopDbContext eShopDbContext)
        {
            _logger = logger;
            _eShopeDbContext = eShopDbContext;
        }

        public IActionResult Index()
        {
            IList<Product> products = _eShopeDbContext.Products.ToList();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        #region Method or Action to return the ProductDetails view with Product Instance 

        public IActionResult ProductDetails(int Id)
        {
            var Product = _eShopeDbContext.Products.FirstOrDefault(p => p.Id == Id);
            return View(Product);
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
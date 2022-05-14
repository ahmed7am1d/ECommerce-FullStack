
using UTBEShop.Models.Entities;
namespace UTBEShop.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public IList<Product> products { get; set; }
        public Product product { get; set; }
    }
}

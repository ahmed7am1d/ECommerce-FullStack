using UTBEShop.Models.Entities;

namespace UTBEShop.Models.Infrastructure.Database
{
    public class DatabaseFake
    {
        public static List<Product> products { get; set; }

        static DatabaseFake()
        {
            DatabaseInit dbInit = new DatabaseInit();
            products = dbInit.GenerateFakeProducts();
        }


    }
}

using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using UTBEShop.Models.Entities;
using UTBEShop.Models.Entities.Identity;

namespace UTBEShop.Models.Infrastructure.Database
{
    public class DatabaseInit
    {

        #region Method Return List of Seeding Data
        public List<Product> GenerateFakeProducts()
        {
            List<Product> products = new List<Product>()
            {
                new Product(1,"Nivida RTX 3060","GPU card with 6GB RAM and tomato",310,"/img/GPU1.jpg","Electornics"),
                new Product(2,"Nivida 1660TI","GPU card with 6GB RAM and tomato",250,"/img/GPU2.jpg","Electornics, Computers"),
                new Product(3,"Cooler","Fan PC Cooler 60W",140,"/img/COOLER1.jpg","Electornics, Computers"),
                new Product(4,"PC","fully Compatiable PC wiht new gpu and cpu",950,"/img/pc1.jpg","Full PC"),

            };

            return products;
        }
        #endregion

        #region Method return list of Role to seed the database on Creating 
        public List<Role> GenerateRoles()
        {
            //we have 3 rules admin,manager,customer
            //each Role accepts the role name and ID
            Role admin = new Role(Roles.Admin.ToString(), 1);
            Role manager =new Role(Roles.Manager.ToString(), 2);
            Role Customer = new Role(Roles.Customer.ToString(), 3);
            List<Role> roles = new List<Role>()
            {
                admin, manager, Customer
            };
            return roles;
        }
        #endregion

        #region Method to insert Admin to the database when the first the app is built (one time only)
        //if only database does not contain admin we will do it 

        public async Task EnsureAdminCreated(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "admin",
                Email = "admin@admin.cz",
                EmailConfirmed = true,
                FirstName = "name",
                LastName = "last name"
            };
            string password = "AhmedAdmin09";

            User adminInDatabase = await userManager.FindByNameAsync(user.UserName);

            if (adminInDatabase == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result == IdentityResult.Success)
                {
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Admin: {error.Code}, {error.Description}");
                    }
                }
            }

        }

        #endregion

        #region Method to Insert Manager to the database when the first the app is built (only one time) 
        //if only database does not contain admin we will do it 
        public async Task EnsureManagerCreated(UserManager<User> userManager)
        {
            User user = new User
            {
                UserName = "manager",
                Email = "manager@manager.cz",
                EmailConfirmed = true,
                FirstName = "name",
                LastName = "last name"
            };
            string password = "abc";

            User managerInDatabase = await userManager.FindByNameAsync(user.UserName);

            if (managerInDatabase == null)
            {

                IdentityResult result = await userManager.CreateAsync(user, password);

                if (result == IdentityResult.Success)
                {
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        if (role != Roles.Admin.ToString())
                            await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Manager: {error.Code}, {error.Description}");
                    }
                }
            }

        }
        #endregion

    }
}

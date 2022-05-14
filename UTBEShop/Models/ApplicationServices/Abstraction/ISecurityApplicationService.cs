using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UTBEShop.Models.Entities.Identity;
using UTBEShop.Models.ViewModel;


namespace UTBEShop.Models.ApplicationServices.Abstraction
{
    public interface ISecurityApplicationService
    {
        Task<string[]> Register(RegisterViewModel vm, Roles role);
        Task<bool> Login(LoginViewModel vm);
        Task Logout();
        Task<User> FindUserByUsername(string username);
        Task<User> FindUserByEmail(string email);
        Task<IList<string>> GetUserRoles(User user);
        Task<User> GetCurrentUser(ClaimsPrincipal principal);

    }
}

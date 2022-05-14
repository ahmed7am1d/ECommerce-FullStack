using Microsoft.AspNetCore.Mvc;
using UTBEShop.Models.ApplicationServices.Abstraction;
using UTBEShop.Models.ViewModel;
using UTBEShop.Models.Infrastructure.Database;
namespace UTBEShop.Areas.Security.Controllers
{
    //Adding the routing 
    [Area("Security")]
    public class AccountController : Controller
    {
        //-- Dependecny Injection for the services of security to handle users and recivve info about them 
        //-- ASP automatically will know and map this dependecny injection 
        ISecurityApplicationService _securityService;

        public AccountController(ISecurityApplicationService securityService)
        {
               _securityService = securityService;
        }


        #region -- Action Register
        public IActionResult Register()
        {
            //_securityService.Register()
            
            return View();
        }
        [HttpPost] 
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                string[] errors = await _securityService.Register(registerViewModel, Models.Entities.Identity.Roles.Customer);
                //if no errors returend we make him login directly 
                if (errors == null)
                {
                    LoginViewModel loginViewModel = new LoginViewModel()
                    {
                        Username = registerViewModel.Username,
                        Password = registerViewModel.Password,
                        LoginFailed = false
                    };
                    bool success = await _securityService.Login(loginViewModel);
                    if(success)
                    {
                        //make him return to the main page if login is successful
                        return RedirectToAction("Index", "Home",new {area = String.Empty});
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    }

                }
            }
            return View(registerViewModel);
        }


        #endregion

        #region -- Action Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                bool success = await _securityService.Login(loginViewModel);
                if (success)
                {
                    return RedirectToAction("Index","Home",new {area = String.Empty});
                }
                loginViewModel.LoginFailed = true;
            }

            return View(loginViewModel);
        }

        public IActionResult Login()
        {
            return View();
        }

        #endregion

        #region -- Action Logout
        public async Task<IActionResult> Logout()
        {
            await _securityService.Logout();
            return RedirectToAction(nameof(Login));
        }
        #endregion


    }
}

using Microsoft.AspNetCore.Identity;

namespace UTBEShop.Models.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        //Identity User already has password, Email, PasswordHash , ......
        #region Proprties 
        public virtual string? FirstName { get; set; }
        public virtual string? LastName { get; set; }
        #endregion


    }
}

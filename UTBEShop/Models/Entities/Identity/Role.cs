using Microsoft.AspNetCore.Identity;

namespace UTBEShop.Models.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        #region Constructor
        public Role()
        {

        }

        //because Role inherit from IdentityRole and it has proprety name
        //check IdentityRole it has proprties and ,,,,,,
        public Role(string name, int id) : base(name)
        {
            Id = id;
            NormalizedName = name.ToUpper();
        }
        #endregion


    }
}

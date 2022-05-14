using Newtonsoft.Json;

namespace UTBEShop.Models.Extensions
{
    /// <summary>
    ///extensions for session
    /// </summary>
    public static class SessionExtensions
    {
        #region GetDouble Method 

        public static double? GetDouble(this ISession session, string key)
        {
            //example of sended key => totalPriceString 
            //return in this keys => is the total prcie in the current session (because each add to cart will update the value) 
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToSingle(data, 0);
        }


        #endregion


        #region SetDouble Method
        /*
         Example of this method in the controller when user add to his cart is: 
            -- key (always = TotalPrice)
            -- value (current total price of the user cart ) 
         */
        public static void SetDouble(this ISession session , string key , double value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }


        #endregion


        #region GetObject Method 
        //this method will return any object from the session such as array or List,,, etc 
        //the key here will be always (OrderItems) as provided the CustomerOrderNotCart
        //whatever is type of object will be converted to and from json string 
        public static T GetObject<T>(this ISession session,string key)
        {
            var data = session.GetString(key);
            if(data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        #endregion


        #region SetObject Method 
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key,JsonConvert.SerializeObject(value));
        }
        #endregion


    }
}

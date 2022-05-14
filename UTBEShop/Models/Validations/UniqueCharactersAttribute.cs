using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace UTBEShop.Models.Validations
{
    public class UniqueCharactersAttribute : ValidationAttribute
    {
        //IClientModelValidator for client-side validation

        #region Method to check if the sended string has unique characters 
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null && String.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"The Field Can not be empty or containing white spaces !!");
            }
            //else we check that the sended value must contains unique characters 
            else
            {
                if(IsUniqueString(value.ToString()))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"All Character entered in the Field Must Be Unique !!");

                }
            }
            throw new NotImplementedException();
        }
        #endregion

        #region Method that will check a string has unique characters 

        private bool IsUniqueString(string userString)
        {
            bool IsUnique = true;
            Hashtable userCharacters = new Hashtable();
            for(int i = 0; i < userString.Length; i++)
            {
                if(userCharacters.ContainsKey(userString[i]))
                {
                    IsUnique = false;
                    break;
                }
                else
                {
                    userCharacters.Add(userString[i], 1);
                }
            }

            return IsUnique;
           
        }

        #endregion

        #region Method for adding validation - that added by us and will be linked in razor page 
        //these can be added to the html element as attributes

        ////this method is belong to the interface => IClientModelValidator
        //public void AddValidation (ClientModelValidationContext context)
        //{
        //    context.Attributes.Add("data-val", "true");
        //    context.Attributes.Add("data-val-filecontent",$"")
    }
    #endregion
}


using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UTBEShop.Models.Validations
{
    //Here we are specifying that our cutsom attribute will and can accepts (Field, proprety, parameter) 
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter )]
    public class FileContentAttribute : ValidationAttribute , IClientModelValidator
    {
        //Dependecy injection
        public string ContentType { get; set; }
        
        public FileContentAttribute(string contentType)
        {
            ContentType = contentType;
        }

        #region Method to check if the sended content type is vaild
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //it is fine maybe user does not want to upload an image
            if(value ==null)
            {
                return ValidationResult.Success;
            }
            else if (value is IFormFile formFile)
            {
                if (formFile.ContentType.ToLower().Contains(this.ContentType.ToLower()))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"{validationContext.MemberName}: Content of the file is not {ContentType}");
                }
            }
            throw new NotImplementedException($"The Attribute {nameof(ContentType)} is not Implemented for {value.GetType()}");
        }
        #endregion

        #region Method for adding validation - that added by us and will be linked in razor page 
        public void AddValidation(ClientModelValidationContext context)
        {
            //these can be added to the html element as attributes
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-filecontent",$"Content of the file is not {ContentType}");
            context.Attributes.Add("data-val-filecontent-type",ContentType.ToLower());
        }
        #endregion


    }
}

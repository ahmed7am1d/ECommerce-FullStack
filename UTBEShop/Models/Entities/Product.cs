using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using UTBEShop.Models.Validations;

namespace UTBEShop.Models.Entities
{
    //Create the table name in the database depened what is our class name 
    [Table(nameof(Product))]
    public class Product
    {
        #region Members Varaiables
        [Key]
        [Required]
        public int Id { get; set; }
        //Validation and requiremnet 
        [StringLength(30)]
        [Required]
        public string ProductName { get; set; }
        [StringLength(255)]
        [Required]
        public string ProductDescription { get; set; }
       
        [Required]
        public double ProductPrice { get; set; }


        [NotMapped]
        [FileContent("image")]
        public IFormFile Image { get; set; }

        public string ProductSourceImage { get; set; }
        [Required]
        //[UniqueCharacters]
        public string ProductType { get; set; }
        #endregion

        #region Constructor
        public Product(int id, string productName, string productDescription, double productPrice, string productSourceImage, string productType)
        {
            this.Id = id;
            this.ProductName = productName;
            this.ProductDescription = productDescription;
            this.ProductPrice = productPrice;
            this.ProductSourceImage = productSourceImage;
            this.ProductType = productType;
        }

        public Product()
        {

        }

        
        #endregion

    }
}

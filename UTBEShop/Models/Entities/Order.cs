using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UTBEShop.Models.Entities.Identity;

namespace UTBEShop.Models.Entities
{

    [Table(nameof(Order))]
    public class Order
    {
        [Key]
        [Required]
        public int ID { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateTimeCreated { get; protected set; }
        
        [StringLength(25)]
        [Required]
        public string OrderNumber { get; set; }

        [Required]
        public double TotalPrice { get; set; }


        //ASP.NET is able to autodetect the User instance and bring from other table 
        //there is no need to bring it by ourself by searhing for the id in the database 


        [ForeignKey(nameof(User))]
        public int UserID { get; set; }
        public User user { get; set; }

        public IList<OrderItem> OrderItems { get; set; }



    }
}

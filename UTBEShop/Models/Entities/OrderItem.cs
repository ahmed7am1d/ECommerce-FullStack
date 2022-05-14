using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTBEShop.Models.Entities
{
    //Reminder => each order will have a list of orderitems :) 
    [Table(nameof(OrderItem))]
    public class OrderItem
    {

        [Key]
        [Required]
        public int ID { get; set; }

        [ForeignKey(nameof(Order))] 
        public int OrderID { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductID { get; set; }

        public int Amount { get; set; }
        public double Price { get; set; }
        public Order order { get; set; }
        public Product product { get; set; }


    }
}

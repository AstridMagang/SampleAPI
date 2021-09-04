using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class Order: ITracking
    {
        public Order()
        {
            Id = Guid.NewGuid().ToString();
            Date = DateTime.Now;
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
            ListOrderItem = new HashSet<OrderItem>();
        }

        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "OrderNo is required")]
        [StringLength(20)]
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "CustomerId is required")]
        public string CustomerId { get; set; }
        [Required(ErrorMessage = "CustomerName is required")]
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; } 
        public string Notes { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public ICollection<OrderItem> ListOrderItem { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }

        public static implicit operator Order(string v)
        {
            throw new NotImplementedException();
        }
    }
}

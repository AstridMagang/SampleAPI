using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Test.Models
{
    public class OrderItem: ITracking
    {
        public OrderItem()
        {
            Id = Guid.NewGuid().ToString();
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }
        [Key]
        public string Id { get; set; }
        public string OrderId { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string ProductCode { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public string UnitId { get; set; }
        [Required]
        public string UnitName { get; set; }
        [Required]
        public decimal Price { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public string DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal { get; set; }
        [JsonIgnore]
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [JsonIgnore]
        public virtual Unit Unit { get; set; }
        [JsonIgnore]
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    public class Product: ITracking
    {
        public Product()
        {
            Id = Guid.NewGuid().ToString();
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }

        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "ProductCode is required")]
        [StringLength(20)]
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "UnitId is required")]
        public string UnitId { get; set; }
        [Required(ErrorMessage = "UnitName is required")]
        public string UnitName { get; set; }
        public bool IsActive { get; set; }
        public bool IsTax { get; set; }
        [ForeignKey("UnitId")]
        public virtual Unit Unit { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}

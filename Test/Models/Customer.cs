using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test.Models
{
    public class Customer : ITracking
    {
        public Customer()
        {
            Id = Guid.NewGuid().ToString();
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }

        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "CustomerCode is required")]
        [StringLength(20)]
        public string CustomerCode { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string NoKK { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Village { get; set; }
        public string Districts { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string FullAddress { get; set; }
        public string NPWP { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public ICollection<Order> ListOrder { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}

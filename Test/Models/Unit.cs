using System;
using System.ComponentModel.DataAnnotations;
namespace Test.Models
{
    public class Unit: ITracking
    {
        public Unit()
        {
            Id = Guid.NewGuid().ToString();
            CreatedTime = DateTime.Now;
            ModifiedTime = DateTime.Now;
        }
        [Key]
        public string Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(10)]
        public string Name { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}

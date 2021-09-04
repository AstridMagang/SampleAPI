using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Models;

namespace Test.DTO.ProductDTO
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string ProductCode { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public bool IsActive { get; set; }
        public bool IsTax { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public Unit Unit { get; set; }
        public static Expression<Func<Product, ProductDTO>> SELECT = x => new ProductDTO
        {
            Id = x.Id,
            Name = x.Name,
            ProductCode = x.ProductCode,
            Barcode = x.Barcode,
            Price = x.Price,
            UnitId = x.UnitId,
            UnitName = x.UnitName,
            IsActive = x.IsActive,
            IsTax = x.IsTax,
            CreatedTime = x.CreatedTime,
            ModifiedTime = x.ModifiedTime,
            Unit = x.Unit,
        };
    }
}

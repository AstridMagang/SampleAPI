using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Models;

namespace Test.DTO.OrderDTO
{

    public class OrderItemDTO
    {
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }
        public string UnitId { get; set; }
        public string UnitName { get; set; }
        public decimal Price { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public string DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal LineTotal { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }

        public static Expression<Func<OrderItem, OrderItemDTO>> SELECT = x => new OrderItemDTO
        {

            Id = x.Id,
            OrderId = x.OrderId,
            ProductId = x.ProductId,
            ProductCode = x.ProductCode,
            ProductName = x.ProductName,
            Quantity = x.Quantity,
            UnitId = x.UnitId,
            UnitName = x.UnitName,
            Price = x.Price,
            LineTotal = x.LineTotal,
            DiscountAmount = x.DiscountAmount,
            DiscountPercentage = x.DiscountPercentage,
            TaxAmount = x.TaxAmount,
            TaxPercentage = x.TaxPercentage,
            CreatedTime = x.CreatedTime,
            ModifiedTime = x.ModifiedTime
        };
    }
}

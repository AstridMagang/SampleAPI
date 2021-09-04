using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Models;

namespace Test.DTO.OrderDTO
{
    public class OrderDTO
    {
        public string Id { get; set; }
        public string OrderNo { get; set; }
        public DateTime Date { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public ICollection<OrderItemDTO> ListOrderItem { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }

        public static Expression<Func<Order, OrderDTO>> SELECT = x => new OrderDTO
        {
            
            Id = x.Id,
            OrderNo = x.OrderNo,
            Date = x.Date,
            CustomerId = x.CustomerId,
            CustomerName = x.CustomerName,
            SubTotal = x.SubTotal,
            DiscountAmount = x.DiscountAmount,
            DiscountPercentage = x.DiscountPercentage,
            TaxAmount =x.TaxAmount,
            TaxPercentage = x.TaxPercentage,
            Total = x.Total,
            Notes = x.Notes,
            CreatedTime = x.CreatedTime,
            ModifiedTime = x.ModifiedTime,
            ListOrderItem = x.ListOrderItem.AsQueryable().Select(OrderItemDTO.SELECT).ToList(),
        };
    }
}

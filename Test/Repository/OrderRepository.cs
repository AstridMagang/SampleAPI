using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DAL;
using Test.DTO.OrderDTO;
using Test.Models;

namespace Test.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext db;
        private bool disposed;

        public OrderRepository(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IQueryable<OrderDTO> GetOrders()
        {
            var props = db.Orders
                .Select(OrderDTO.SELECT)
                .AsQueryable();
            return props;
        }

        public async Task<OrderDTO> GetOrderByID(string Id)
        {
            var props = await db.Orders
                .Select(OrderDTO.SELECT)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public async Task<Order> GetOrderByModelId(string Id)
        {
            var props = await db.Orders.Include(a => a.ListOrderItem)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public bool GetCustomerId(string Id)
        {
            var props = db.Orders
                .Any(x => x.CustomerId == Id);
            return props;
        }

        public void InsertOrder(Order Order)
        {
            db.Orders.Add(Order);
        }

        public void DeleteOrder(Order Order)
        {
            var orderitems = db.OrderItems.Where(a => a.OrderId == Order.Id);
            db.OrderItems.RemoveRange(orderitems);
            db.Orders.Remove(Order);
        }

        public void UpdateOrder(Order Order)
        {
            db.Entry(Order).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    db.Dispose();
            disposed = true;
        }

        
        public Order GetOrderByIDModel(string Id)
        {
            var props = db.Orders
               .Where(x => x.Id == Id).FirstOrDefault();
            return props;
        }

        public string GetLastOrderNO()
        {
            var props = db.Orders.OrderByDescending(a => a.OrderNo).FirstOrDefault()?.OrderNo ?? "";
            return props;
        }
    }
}

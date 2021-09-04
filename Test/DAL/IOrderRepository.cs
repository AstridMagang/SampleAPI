using System;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.OrderDTO;
using Test.Models;

namespace Test.DAL
{
    public interface IOrderRepository
    {
        IQueryable<OrderDTO> GetOrders();
        Task<OrderDTO> GetOrderByID(string OrderId);
        void InsertOrder(Order Order);
        void UpdateOrder(Order Order);
        void DeleteOrder(Order Order);
        string GetLastOrderNO();
        Task SaveAsync();
    }
}

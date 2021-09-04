using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.CustomerDTO;
using Test.Models;

namespace Test.DAL
{
    public interface ICustomerRepository: IDisposable
    {
        List<CustomerDTO> GetCustomers();
        Task<CustomerDTO> GetCustomerByID(string CustomerId);
        void InsertCustomer(Customer Customer);
        void UpdateCustomer(Customer Customer);
        void DeleteCustomer(Customer Customer);
        Task<Customer> GetCustomerByIDModel(string Id);
        bool CustomerExists(string Id);
        Task SaveAsync();
    }
}

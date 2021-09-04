using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.DAL;
using Test.DTO.CustomerDTO;
using Test.Helper;
using Test.Models;

namespace Test.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext db;
        private readonly CacheHelper cacheHelper;
        private bool disposed;

        public CustomerRepository(ApplicationDbContext db)
        {
            this.db = db;
            cacheHelper = new CacheHelper();
        }

        public List<CustomerDTO> GetCustomers()
        {
            var key = "Customers";
            var props = cacheHelper.GetMemCache<List<CustomerDTO>>(key);
            if (props == null || props.Count() == 0)
            {
                props = db.Customers
                    .Select(CustomerDTO.SELECT)
                    .OrderBy(a => a.Name).ToList();
            }
            cacheHelper.UpdateMemCache(key, props, DateTimeOffset.UtcNow.AddDays(7));

            return props;
        }

        public async Task<CustomerDTO> GetCustomerByID(string Id)
        {
            var props = await db.Customers
                .Select(CustomerDTO.SELECT)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public async Task<Customer> GetCustomerByIDModel(string Id)
        {
            var props = await db.Customers
                .SingleOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public void InsertCustomer(Customer Customer)
        {
            db.Customers.Add(Customer);
        }

        public void DeleteCustomer(Customer Customer)
        {
            db.Customers.Remove(Customer);
        }

        public void UpdateCustomer(Customer Customer)
        {
            db.Entry(Customer).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public bool CustomerExists(string Id)
        {
            return db.Customers.Count(x => x.Id == Id) > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<CustomerDTO> GetCustomerPercentage()
        {
            var props = await db.Customers
                .Select(CustomerDTO.SELECT)
                .FirstOrDefaultAsync();
            return props;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    db.Dispose();
            disposed = true;
        }
    }
}

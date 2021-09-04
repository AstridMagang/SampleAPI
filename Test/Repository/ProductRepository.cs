using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DAL;
using Test.DTO.ProductDTO;
using Test.Helper;
using Test.Models;

namespace Test.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext db;
        private readonly CacheHelper cacheHelper;
        private bool disposed;

        public ProductRepository(ApplicationDbContext db)
        {
            this.db = db;
            cacheHelper = new CacheHelper();
        }

        public List<ProductDTO> GetProducts(List<string> listId)
        {
            var key = "Products";
            var props = cacheHelper.GetMemCache<List<ProductDTO>>(key);
            if (props == null || props.Count() == 0)
            {
                props = db.Products
                    .Select(ProductDTO.SELECT)
                    .OrderBy(a => a.Name).ToList();
            }
            cacheHelper.UpdateMemCache(key, props, DateTimeOffset.UtcNow.AddDays(7));

            if (listId.Any())
                props = props.Where(a => listId.Contains(a.Id)).ToList();

            return props;
        }

        public async Task<ProductDTO> GetProductByID(string Id)
        {
            var props = await db.Products
                .Select(ProductDTO.SELECT)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public async Task<Product> GetProductByIDModel(string Id)
        {
            var props = await db.Products
                .SingleOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public void InsertProduct(Product Product)
        {
            db.Products.Add(Product);
        }

        public void DeleteProduct(Product Product)
        {
            db.Products.Remove(Product);
        }

        public void UpdateProduct(Product Product)
        {
            db.Entry(Product).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public bool ProductExists(string Id)
        {
            return db.Products.Count(x => x.Id == Id) > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<ProductDTO> GetProductPercentage()
        {
            var props = await db.Products
                .Select(ProductDTO.SELECT)
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

        public bool GetUnits(string unitId)
        {
            var props = db.Products
                .Any(a => a.UnitId == unitId);
            return props;
        }
    }
}

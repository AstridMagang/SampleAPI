using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.ProductDTO;
using Test.Models;

namespace Test.DAL
{
    public interface IProductRepository : IDisposable
    {
        List<ProductDTO> GetProducts(List<string> listId);
        Task<ProductDTO> GetProductByID(string ProductId);
        void InsertProduct(Product Product);
        void UpdateProduct(Product Product);
        void DeleteProduct(Product Product);
        Task<Product> GetProductByIDModel(string Id);
        bool ProductExists(string Id);
        bool GetUnits(string Id);
        Task SaveAsync();
    }
}

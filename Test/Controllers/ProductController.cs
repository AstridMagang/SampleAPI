using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.ProductDTO;
using Test.Helper;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Product")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly CacheHelper cacheHelper;
        private readonly ProductRepository ProductRepository;

        public ProductController(ApplicationDbContext db)
        {
            this.db = db;
            cacheHelper = new CacheHelper();
            ProductRepository = new ProductRepository(db);
        }


        #region Get All

        [HttpGet("GetAll")]
        public List<ProductDTO> GetAll(List<string> listid = null)
        {
            return
                ProductRepository
                    .GetProducts(listid);
        }

        #endregion

        #region Get ProductById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var results = await ProductRepository.GetProductByID(Id);
            if (results == null) return NotFound();
            return Ok(results);
        }

        #endregion

        #region Post Product

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Product Product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await using var trx = db.Database.BeginTransaction();
            try
            {
                Product.ProductCode = Product.ProductCode.ToUpper().Trim();
                Product.Barcode = Product.Barcode.Trim();
                Product.Name = Product.Name.Trim();
                ProductRepository.InsertProduct(Product);
                await ProductRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Products");
                return Ok(Product);
            }
            catch (DataException ex)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, e.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        #endregion

        #region Put Product

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string Id, [FromBody] Product Product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (Id != Product.Id)
            {
                ModelState.AddModelError(Constanta.ERROR_MARKER, "Id mismatch!");
                return BadRequest();
            }

            await using var trx = db.Database.BeginTransaction();
            try
            {
                Product.ProductCode = Product.ProductCode.ToUpper().Trim();
                Product.Barcode = Product.Barcode.Trim();
                Product.Name = Product.Name.Trim();
                Product.ModifiedTime = DateTime.Now;
                ProductRepository.UpdateProduct(Product);
                await ProductRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Products");
                return Ok(Product);
            }
            catch (DbUpdateConcurrencyException e)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, e.GetBaseException().Message);
                return BadRequest(ModelState);
            }
            catch (DataException ex)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, e.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string Id)
        {
            var Product = await ProductRepository.GetProductByIDModel(Id);
            if (Product == null) return NotFound();

            await using var trx = db.Database.BeginTransaction();
            try
            {
                ProductRepository.DeleteProduct(Product);
                await ProductRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Products");
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, ex.GetBaseException().Message);
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                trx.Rollback();
                ModelState.AddModelError(Constanta.ERROR_MARKER, e.GetBaseException().Message);
                return BadRequest(ModelState);
            }
        }

        #endregion

        #region DISPOSE

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

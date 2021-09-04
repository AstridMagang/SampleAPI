using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.OrderDTO;
using Test.Helper;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly OrderRepository OrderRepository;
        private readonly ProductRepository ProductRepository;
        public IConfiguration Configuration { get; }

        public OrderController(ApplicationDbContext db, IConfiguration config)
        {
            this.db = db;
            OrderRepository = new OrderRepository(db);
            ProductRepository = new ProductRepository(db);
            Configuration = config;
        }


        #region Get All

        [HttpGet("GetAll")]
        public IQueryable<OrderDTO> GetAll()
        {
            return
                OrderRepository
                    .GetOrders();
        }

        #endregion

        #region Get OrderById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var results = await OrderRepository.GetOrderByID(Id);
            if (results == null) return NotFound();
            return Ok(results);
        }

        #endregion

        #region Post Order

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order Order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!Order.ListOrderItem.Any()) return BadRequest("Please Input Items");

            await using var trx = db.Database.BeginTransaction();
            try
            {
                PostPut(Order);
                OrderRepository.InsertOrder(Order);
                await OrderRepository.SaveAsync();
                trx.Commit();
                return Ok(Order);
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

        private void PostPut(Order Order)
        {
            var Items = Order.ListOrderItem.ToList();
            var ProductIds = Items.Select(a => a.ProductId).ToList();
            var listProduct = ProductRepository.GetProducts(ProductIds);

            foreach (var item in Items)
            {
                var productPPN = listProduct.SingleOrDefault(a => a.Id == item.ProductId).IsTax;
                item.TaxPercentage = productPPN ? 10.0m : 0.0m;
                item.TaxAmount = productPPN ? (item.Price * 10.0m) / 100 : 0.0m;
                item.LineTotal = item.Price * item.Quantity;
                item.DiscountAmount = (item.LineTotal * decimal.Parse(item.DiscountPercentage)) / 100;
                item.LineTotal = item.LineTotal - item.DiscountAmount;
            }

            Order.SubTotal = Items.Sum(a => a.LineTotal);
            var anyTaxitem = Items.Any(a => a.TaxPercentage > 0.0m);
            Order.TaxPercentage = anyTaxitem ? 10.0m : 0.0m;
            Order.TaxAmount = Items.Sum(a => a.TaxAmount);
            Order.Total = Order.SubTotal + Order.TaxAmount;
            Order.ModifiedTime = DateTime.Now;
        }

        #endregion

        #region Put Order

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string Id, [FromBody] Order Order)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!Order.ListOrderItem.Any()) return BadRequest("Please Input Items");

            if (Id != Order.Id)
            {
                ModelState.AddModelError(Constanta.ERROR_MARKER, "Id mismatch!");
                return BadRequest();
            }

            await using var trx = db.Database.BeginTransaction();
            try
            {
                PostPut(Order);
                OrderRepository.UpdateOrder(Order);
                await OrderRepository.SaveAsync();
                trx.Commit();
                return Ok(Order);
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
            var Order = OrderRepository.GetOrderByIDModel(Id);
            if (Order == null) return NotFound("Data Not Found");

            await using var trx = db.Database.BeginTransaction();
            try
            {
                OrderRepository.DeleteOrder(Order);
                await OrderRepository.SaveAsync();
                trx.Commit();
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

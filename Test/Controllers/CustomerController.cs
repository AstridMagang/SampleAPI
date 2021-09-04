using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Test.DTO.CustomerDTO;
using Test.Helper;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Customer")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly CacheHelper cacheHelper;
        private readonly CustomerRepository CustomerRepository;
        private readonly OrderRepository orderRepository;

        public CustomerController(ApplicationDbContext dbContext)
        {
            CustomerRepository = new CustomerRepository(dbContext);
            orderRepository = new OrderRepository(dbContext);
            cacheHelper = new CacheHelper();
            db = dbContext;
        }


        #region Get All
        [HttpGet("GetAll")]
        public List<CustomerDTO> GetAll()
        {
            return CustomerRepository
                    .GetCustomers();
        }

        #endregion

        #region Get CustomerById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var results = await CustomerRepository.GetCustomerByID(Id);
            if (results == null) return NotFound();
            return Ok(results);
        }

        #endregion

        #region Post Customer

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer Customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await using var trx = db.Database.BeginTransaction();
            try
            {
                Customer.CustomerCode = Customer.CustomerCode.ToUpper().Trim();
                Customer.Name = Customer.Name.Trim();
                CustomerRepository.InsertCustomer(Customer);
                await CustomerRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Customers");
                return Ok(Customer);
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

        #region Put Customer

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string Id, [FromBody] Customer Customer)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (Id != Customer.Id)
            {
                ModelState.AddModelError(Constanta.ERROR_MARKER, "Id mismatch!");
                return BadRequest();
            }

            await using var trx = db.Database.BeginTransaction();
            try
            {
                Customer.ModifiedTime = DateTime.Now;
                Customer.CustomerCode = Customer.CustomerCode.ToUpper().Trim();
                Customer.Name = Customer.Name.Trim();
                CustomerRepository.UpdateCustomer(Customer);
                await CustomerRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Customers");
                return Ok(Customer);
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
            var Customer = await CustomerRepository.GetCustomerByIDModel(Id);
            if (Customer == null) return NotFound();

            await using var trx = db.Database.BeginTransaction();
            try
            {
                var soexist = orderRepository.GetCustomerId(Id);
                if (soexist)
                    throw new Exception("Customer tidak boleh didelete karena sudah ada transaksi");
                CustomerRepository.DeleteCustomer(Customer);
                await CustomerRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Customers");
                return Ok("Success Delete");
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

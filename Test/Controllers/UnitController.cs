using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.UnitDTO;
using Test.Helper;
using Test.Models;
using Test.Repository;

namespace Test.Controllers
{
    [Produces("application/json")]
    [Route("api/Unit")]
    public class UnitController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly CacheHelper cacheHelper;
        private readonly UnitRepository UnitRepository;
        private readonly ProductRepository ProductRepository;

        public UnitController(ApplicationDbContext db)
        {
            this.db = db;
            UnitRepository = new UnitRepository(db);
            ProductRepository = new ProductRepository(db);
            cacheHelper = new CacheHelper();
        }

        #region Get All

        [HttpGet("GetAll")]
        public List<UnitListDTO> GetAll()
        {
            return
                UnitRepository
                    .GetUnits();
        }

        #endregion

        #region Get UnitById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string Id)
        {
            var results = await UnitRepository.GetUnitByID(Id);
            if (results == null) return NotFound();
            return Ok(results);
        }

        #endregion

        #region Post Unit

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Unit Unit)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await using var trx = db.Database.BeginTransaction();
            try
            {
                Unit.Name = Unit.Name.ToUpper().Trim();
                UnitRepository.InsertUnit(Unit);
                await UnitRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Units");
                return Ok(Unit);
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

        #region Put Unit

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] string Id, [FromBody] Unit Unit)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (Id != Unit.Id)
            {
                ModelState.AddModelError(Constanta.ERROR_MARKER, "Id mismatch!");
                return BadRequest();
            }

            await using var trx = db.Database.BeginTransaction();
            try
            {
                Unit.ModifiedTime = DateTime.Now;
                Unit.Name = Unit.Name.ToUpper().Trim();
                var oldUnit = await db.Units.FindAsync(Id);
                db.Entry(oldUnit).CurrentValues.SetValues(Unit);
                await UnitRepository.SaveAsync();
                trx.Commit();
                cacheHelper.RemoveMemchace("Units");
                return Ok(Unit);
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
            var Unit = await UnitRepository.GetUnitByIDModel(Id);
            if (Unit == null) return NotFound("Data Not Found");

            await using var trx = db.Database.BeginTransaction();
            try
            {
                var exist = ProductRepository.GetUnits(Id);
                if (exist)
                    throw new Exception("Unit tidak boleh didelete karena ada product yang pakai");
                UnitRepository.DeleteUnit(Unit);
                await UnitRepository.SaveAsync();
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

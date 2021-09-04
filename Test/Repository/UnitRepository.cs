using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DAL;
using Test.DTO.UnitDTO;
using Test.Helper;
using Test.Models;

namespace Test.Repository
{
    public class UnitRepository : IUnitRepository
    {
        private readonly ApplicationDbContext db;
        private readonly CacheHelper cacheHelper;
        private readonly ProductRepository productRepository;
        private bool disposed;

        public UnitRepository(ApplicationDbContext db)
        {
            this.db = db;
            cacheHelper = new CacheHelper();
            productRepository = new ProductRepository(db);
        }

        public List<UnitListDTO> GetUnits()
        {
            var key = "Units";
            var props = cacheHelper.GetMemCache<List<UnitListDTO>>(key);
            if (props == null || props.Count() == 0)
            {
                props = db.Units
                    .Select(UnitListDTO.SELECT)
                    .OrderBy(a => a.Name).ToList();
            }
            cacheHelper.UpdateMemCache(key, props, DateTimeOffset.UtcNow.AddDays(7));

            return props;
        }

        public async Task<UnitDTO> GetUnitByID(string Id)
        {
            var props = await db.Units
                .Select(UnitDTO.SELECT)
                .FirstOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public async Task<Unit> GetUnitByIDModel(string Id)
        {
            var props = await db.Units
                .SingleOrDefaultAsync(x => x.Id == Id);
            return props;
        }

        public void InsertUnit(Unit Unit)
        {
            db.Units.Add(Unit);
        }

        public void DeleteUnit(Unit Unit)
        {
            db.Units.Remove(Unit);
        }

        public void UpdateUnit(Unit Unit)
        {
            db.Entry(Unit).State = EntityState.Modified;
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public bool UnitExists(string Id)
        {
            return db.Units.Count(x => x.Id == Id) > 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<UnitDTO> GetUnitPercentage()
        {
            var props = await db.Units
                .Select(UnitDTO.SELECT)
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

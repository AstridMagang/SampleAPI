using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DTO.UnitDTO;
using Test.Models;

namespace Test.DAL
{
    public interface IUnitRepository : IDisposable
    {
        List<UnitListDTO> GetUnits();
        Task<UnitDTO> GetUnitByID(string UnitId);
        void InsertUnit(Unit Unit);
        void UpdateUnit(Unit Unit);
        void DeleteUnit(Unit Unit);
        Task<Unit> GetUnitByIDModel(string Id);
        bool UnitExists(string Id);
        Task SaveAsync();
    }
}

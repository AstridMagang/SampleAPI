using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Models;

namespace Test.DTO.UnitDTO
{
    public class UnitListDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public static Expression<Func<Unit, UnitListDTO>> SELECT = x => new UnitListDTO
        {
            Id = x.Id,
            Name = x.Name,
        };
    }
}

using System;
using System.Linq.Expressions;
using Test.Models;
namespace Test.DTO.UnitDTO
{
    public class UnitDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }

        public static Expression<Func<Unit, UnitDTO>> SELECT = x => new UnitDTO
        {
            Id = x.Id,
            Name = x.Name,
            CreatedTime = x.CreatedTime,
            ModifiedTime = x.ModifiedTime
        };
    }
}

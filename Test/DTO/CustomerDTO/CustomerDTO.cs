using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Models;

namespace Test.DTO.CustomerDTO
{
    public class CustomerDTO
    {
        public string Id { get; set; }
        public string CustomerCode { get; set; }
        public string Name { get; set; }
        public string NoKK { get; set; }
        public string Telephone { get; set; }
        public string Address { get; set; }
        public string Village { get; set; }
        public string Districts { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string FullAddress { get; set; }
        public string NPWP { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public static Expression<Func<Customer, CustomerDTO>> SELECT = x => new CustomerDTO
        {
            Id = x.Id,
            CustomerCode = x.CustomerCode,
            Name = x.Name,
            NoKK = x.NoKK,
            Telephone = x.Telephone,
            Address = x.Address,
            Village = x.Village,
            Districts = x.Districts,
            City = x.City,
            Province = x.Province,
            PostalCode = x.PostalCode,
            FullAddress = x.FullAddress,
            NPWP = x.NPWP,
            Latitude = x.Latitude,
            Longitude = x.Longitude,
            CreatedTime = x.CreatedTime,
            ModifiedTime = x.ModifiedTime
        };
    }
}

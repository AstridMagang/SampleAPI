using System;

namespace Test.Models
{
    public interface ITracking
    {
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
    }
}

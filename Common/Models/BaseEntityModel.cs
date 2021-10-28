using System;

namespace Common.Models
{
    public class BaseEntityModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedTimestamp { get; set; } = DateTime.UtcNow;
    }
}
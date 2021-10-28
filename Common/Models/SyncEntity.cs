using System;
using Microsoft.VisualBasic;

namespace Common.Models
{
    public class SyncEntity
    {
        public Guid Id { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public string JsonData { get; set; }
        public string SyncType { get; set; }
        public string ObjectType { get; set; }
        public string Origin { get; set; }
    }
}
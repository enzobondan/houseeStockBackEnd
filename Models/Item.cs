using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Models
{
    public class Item
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public List<Tag> Tags { get; set; } = [];

        //parent
        public int? ContainerId { get; set; }

        public double? Amount { get; set; }

        public Container? Container { get; set; }

        public string? ImagePath { get; set; }

        //public required string UserId { get; set; }
    }
}
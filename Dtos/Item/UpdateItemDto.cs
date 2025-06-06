using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos.Item
{
    public class UpdateItemDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public string? Description { get; set; }
        
        public List<string> Tags { get; set; } = new List<string>();

        public int? ContainerId { get; set; }

        public double Amount { get; set; }

        public string? ImagePath { get; set; }
    }
}
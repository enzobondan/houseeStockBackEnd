using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Models;

namespace api_stock.Dtos.Item
{
    public class CreateItemDto
    {
        public required string Name { get; set; }

        public string? Description { get; set; }
        
        public List<string> Tags { get; set; } = [];

        public int? ContainerId { get; set; }

        public int? Amount { get; set; }

        public string? ImagePath { get; set; }
    }
}
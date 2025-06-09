using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Models;

namespace api_stock.Dtos
{
    public class ItemDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
        
        public List<string> Tags { get; set; } = [];

        public int? ContainerId { get; set; }

        public int? Amount { get; set; }

        public string? ImagePath { get; set; }

    }
}
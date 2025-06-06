using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos
{
    public class ItemDto
    {

        public required string Name { get; set; }

        public string? Description { get; set; }
        
        public List<string> Tags { get; set; } = new List<string>();

        public int? amount { get; set; }

        //parent
        public int? ContainerId { get; set; }

        public string? ImagePath { get; set; }

    }
}
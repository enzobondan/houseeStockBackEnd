using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_stock.Models;

namespace api_stock.Dtos.Place
{
    public class CreatePlaceDto
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        public List<string> Tags { get; set; } = [];

    }
}
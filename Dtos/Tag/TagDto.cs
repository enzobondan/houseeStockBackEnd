using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos.Tag
{
    public class TagDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
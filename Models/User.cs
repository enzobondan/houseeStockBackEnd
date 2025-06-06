using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api_stock.Models
{
    public class User : IdentityUser
    {

        public List<Place> Places { get; set; } = new List<Place>();
    }
}
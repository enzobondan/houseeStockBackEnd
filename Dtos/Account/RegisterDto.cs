using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos.Account
{
    public class RegisterDto
    {
        public required string Username { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    
        public required string Password { get; set; }
    }
}
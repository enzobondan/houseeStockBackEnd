using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_stock.Dtos.Account
{
    public class LoginDto
    {
        public required string UserName { get; set; }


        public required string Password { get; set; }
    }
}
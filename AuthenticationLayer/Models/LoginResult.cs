﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationLayer.Models
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
    }
}

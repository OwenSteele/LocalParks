﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class TokenService
    {
        public TokenService()
        {

        }

        public bool CheckUser(string username, string password)
        {
            //implement new DB for auth
            return username == "AdminOS_1996" && password == "Cats1nTh3Cradle!";
        }
    }
}

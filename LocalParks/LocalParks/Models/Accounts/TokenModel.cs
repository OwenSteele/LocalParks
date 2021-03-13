using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Models.Accounts
{
    public class TokenModel
    {
        public string Token { get; init; }
        public string Expiry { get; init; }
    }
}

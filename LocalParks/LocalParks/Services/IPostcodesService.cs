using LocalParks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public interface IPostcodesService
    {
        Task<PostcodeModel[]> GetAllPostcodesAsync();
        Task<PostcodeModel> GetPostcodeAsync(string zone);
    }
}

//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.Extensions.Configuration;
//using Microsoft.EntityFrameworkCore;
//using System.IO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace LocalParks.Data
//{
//    public class ParkContextFactory : IDesignTimeDbContextFactory<ParkContext>
//    {
//        // Required due to separation Web App
//        public ParkContext CreateDbContext(string[] args)
//        {
//            var config = new ConfigurationBuilder()
//                .SetBasePath(Directory.GetCurrentDirectory())
//                .AddJsonFile("../LocalParks/appsettings.json")
//                .Build();

//            return new ParkContext(new DbContextOptionsBuilder<ParkContext>().Options, config);
//        }
//    }
//}

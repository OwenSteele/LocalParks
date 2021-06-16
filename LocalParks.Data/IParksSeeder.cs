using System.Threading.Tasks;

namespace LocalParks.Data
{
    public interface IParksSeeder
    {
        Task SeedAsync();
    }
}
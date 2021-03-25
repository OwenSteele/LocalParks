namespace LocalParks.Services.Shared
{
    public interface IRandomService
    {
        int Generate(int max, int min = 0);
    }
}
namespace LocalParks.Core.Contracts.Shared
{
    public interface IRandomService
    {
        int Generate(int max, int min = 0);
    }
}
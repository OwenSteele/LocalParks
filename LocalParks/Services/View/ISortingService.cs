namespace LocalParks.Services.View
{
    public interface ISortingService
    {
        T[] SortResults<T>(T[] models, string sortBy);
    }
}
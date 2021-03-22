using LocalParks.Models.Validation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LocalParks.Services.View
{
    public class SortingService : ISortingService
    {
        public T[] SortResults<T>(T[] models, string sortBy)
        {
            if (typeof(T).GetProperty(sortBy) is not PropertyInfo property)
                return models;

            var sorted = models.Where(p => property.GetValue(p, null) is IComparable);

            if (!sorted.Any()) return models; // -> ICollection.Count

            if (property.GetCustomAttribute<IsSortableAttribute>().Ascending)
                sorted = sorted.OrderBy(p => property.GetValue(p, null));
            else
                sorted = sorted.OrderByDescending(p => property.GetValue(p, null));

            return sorted.ToArray();
        }
    }
}

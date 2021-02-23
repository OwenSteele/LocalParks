using LocalParks.Models.Validation;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace LocalParks.Services.Combined
{
    internal static class SortingService
    {
        internal static string GetDisplayName(PropertyInfo p)
        {
            var attribute = p.GetCustomAttribute<DisplayNameAttribute>();

            if (attribute == null) return p.Name;

            return attribute.DisplayName;
        }
        internal static T[] SortResults<T>(T[] models, string sortBy)
        {
            var property = typeof(T).GetProperty(sortBy);

            if (property == null) return models;

            var sorted = models.Where(p => property.GetValue(p, null) is IComparable);

            if (!sorted.Any()) return models; // -> ICollection.Count

            if (property.GetCustomAttribute<IsSortableAttribute>().Ascending)
                sorted = sorted.OrderBy(p => property.GetValue(p, null));
            else
                sorted = sorted.OrderByDescending(p => property.GetValue(p, null));

            return sorted.ToArray();
        }

        internal static bool IsSortable(PropertyInfo p) =>
            p.GetCustomAttribute(typeof(IsSortableAttribute)) != null;
    }
}

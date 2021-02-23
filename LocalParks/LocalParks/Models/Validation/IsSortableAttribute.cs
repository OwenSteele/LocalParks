using System;

namespace LocalParks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSortableAttribute : Attribute
    {
        public bool Ascending;

        public IsSortableAttribute(bool descending = false)
        {
            Ascending = !descending;
        }
    }
}

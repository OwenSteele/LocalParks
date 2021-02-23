using System;

namespace LocalParks.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IsSortableAttribute : Attribute
    {
        public bool ascending;

        public IsSortableAttribute(bool ascending = true)
        {            
            this.ascending = ascending;
        }
    }
}

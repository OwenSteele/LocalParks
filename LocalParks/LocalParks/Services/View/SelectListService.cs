using AutoMapper;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Models.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LocalParks.Services.View
{
    public class SelectListService : ISelectListService
    {
        public IEnumerable<SelectListItem> GetSortSelectListItems<T>()
        {
            return from p in typeof(T).GetProperties()
                   where IsSortable(p)
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = GetDisplayName(p),
                       Value = p.Name
                   };
        }

        private static string GetDisplayName(PropertyInfo p)
        {
            var attribute = p.GetCustomAttribute<DisplayNameAttribute>();

            if (attribute == null) return p.Name;
            
            return attribute.DisplayName;
        }

        private static bool IsSortable(PropertyInfo p) =>
            p.GetCustomAttribute(typeof(IsSortableAttribute)) != null;
    }
}

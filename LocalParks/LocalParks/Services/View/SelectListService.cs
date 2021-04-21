using AutoMapper;
using LocalParks.Core;
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
        private readonly IParkRepository _parkRepository;
        private readonly IEncryptionService _service;
        private ICollection<Postcode> _postcodes;

        public SelectListService(IParkRepository parkRepository, IEncryptionService service)
        {
            _parkRepository = parkRepository;
            _service = service;
        }
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

        public async Task<IEnumerable<SelectListItem>> GetPostcodeSelectListItemsAsync()
        {
            if (_postcodes == null) _postcodes = await _parkRepository.GetAllPostcodesAsync();

            return from p in _postcodes
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Zone,
                       Value = p.Zone
                   };
        }

        //public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(ParkEventModel model,bool withChildrenOnly = false)
        //{
        //    var parks = _mapper.Map<ICollection<ParkModel>>(await _parkRepository.GetAllParksAsync());

        //    return from p in parks
        //           where !withChildrenOnly || p.Events.Any(e => e.Date >= DateTime.Today)
        //           select new SelectListItem
        //           {
        //               Selected = false,
        //               Text = p.Name,
        //               Value = _service.Encrypt(p.ParkId)
        //           };
        //}

        //public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(SportsClubModel model, bool withChildrenOnly = false)
        //{
        //    var parks = _mapper.Map<ICollection<ParkModel>>(await _parkRepository.GetAllParksAsync());

        //    return from p in parks
        //           where !withChildrenOnly || p.SportClubs.Count > 0
        //           select new SelectListItem
        //           {
        //               Selected = false,
        //               Text = p.Name,
        //               Value = _service.Encrypt(p.ParkId)
        //           };
        //}

        public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync<T>(bool withChildrenOnly = false) where T : class
        {
            var parks = await _parkRepository.GetAllParksAsync();

            return parks.Where(p => !withChildrenOnly ||
            (typeof(T).IsEquivalentTo(typeof(ParkEventModel)) && p.Events.Any(e => e.Date >= DateTime.Today)) |
            (typeof(T).IsEquivalentTo(typeof(SupervisorModel)) && p.Supervisor != null) |
            (typeof(T).IsEquivalentTo(typeof(SportsClubModel)) && p.SportClubs.Count > 0))
                .Select(p => new SelectListItem
                {
                    Selected = false,
                    Text = p.Name,
                    Value = p.ParkId.ToString()
                });
        }
    }
}

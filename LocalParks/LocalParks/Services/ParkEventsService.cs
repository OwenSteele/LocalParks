using AutoMapper;
using LocalParks.Core;
using LocalParks.Data;
using LocalParks.Models;
using LocalParks.Services.Combined;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Services
{
    public class ParkEventsService : IParkEventsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        public ParkEventsService(IParkRepository parkRepository, IMapper mapper)
        {
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ParkEventModel[]> GetAllParkEventModelsAsync(int? parkId = null, string sortBy = null)
        {
            var results = parkId == null ?
                    _mapper.Map<ParkEventModel[]>(await _parkRepository.GetAllEventsAsync()) :
                    _mapper.Map<ParkEventModel[]>(await _parkRepository.GetEventsByParkIdAsync((int)parkId));

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(results, sortBy);

            return results;
        }
        public async Task<ParkEventModel[]> GetSearchedParkEventModelsAsync(
            string searchTerm = null,
            string parkId = null,
            DateTime? date = null,
            string sortBy = null)
        {
            var results = await _parkRepository.GetAllEventsAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();

                results = results.Where(p =>
                p.Name.ToLower() == searchTerm |
                p.Name.ToLower().Contains(searchTerm) |
                p.Name.ToLower().StartsWith(searchTerm))
                    .ToArray();

                if (!results.Any()) return null;
            }
            if (!string.IsNullOrWhiteSpace(parkId))
            {
                var park = int.Parse(parkId);

                results = results.Where(p =>
                p.Park.ParkId == park).ToArray();

                if (!results.Any()) return null;
            }
            if (date != null)
            {
                var dateValue = (DateTime)date;

                results = results.Where(p =>
                p.Date == dateValue.Date)
                    .ToArray();

                if (!results.Any()) return null;
            }

            var models = _mapper.Map<ParkEventModel[]>(results);

            if (!string.IsNullOrWhiteSpace(sortBy))
                return SortingService.SortResults(models, sortBy);

            return models;
        }
        public async Task<ParkEventModel> GetParkEventModelAsync(int parkId, DateTime date)
        {
            var result = await _parkRepository.GetEventByParkIdByDateAsync(parkId, date);

            if (result == null) return null;

            return _mapper.Map<ParkEventModel>(result);
        }
        public async Task<ParkEventModel> GetParkEventModelByIdAsync(int eventId, int? parkId = null)
        {
            var result = await _parkRepository.GetEventByIdAsync(eventId);

            if (result == null) return null;

            if (parkId != null && result.Park.ParkId != parkId) return null;

            return _mapper.Map<ParkEventModel>(result);
        }
        public async Task<IEnumerable<SelectListItem>> GetParkSelectListItemsAsync(bool onlyWithEvents = false)
        {
            var parks = _mapper.Map<ICollection<ParkModel>>(await _parkRepository.GetAllParksAsync());

            return from p in parks
                   where !onlyWithEvents || p.Events.Count > 0
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Name,
                       Value = p.ParkId.ToString()
                   };
        }
        public async Task<ParkModel> GetParkAsync(int parkId)
        {
            var result = await _parkRepository.GetParkByIdAsync(parkId);

            if (result == null) return null;

            return _mapper.Map<ParkModel>(result);
        }

        public IEnumerable<SelectListItem> GetSortSelectListItems()
        {
            return from p in typeof(ParkEventModel).GetProperties()
                   where SortingService.IsSortable(p)
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = SortingService.GetDisplayName(p),
                       Value = p.Name
                   };
        }

        public async Task<ParkEventModel> AddNewParkEventAsync(ParkEventModel model, string username, bool hideUsername = true)
        {
            var user = await _parkRepository.GetLocalParksUserByUsernameAsync(username);

            var email = model.OrganiserEmail.ToLower();
            email = email[..email.IndexOf("@")];

            if (email == "me" ||
                email == "this" ||
                email == "user")
                model.OrganiserEmail = user.Email;

            if (model.OrganiserPhoneNumber.ToLower() == "me" ||
                 model.OrganiserPhoneNumber.ToLower() == "this" ||
                 model.OrganiserPhoneNumber.ToLower() == "user")
                model.OrganiserPhoneNumber = user.PhoneNumber;

            if (model.OrganiserFirstName.ToLower() == "me" ||
                 model.OrganiserFirstName.ToLower() == "this" ||
                 model.OrganiserFirstName.ToLower() == "user")
                model.OrganiserFirstName = user.FirstName;

            if (model.OrganiserLastName.ToLower() == "me" ||
                 model.OrganiserLastName.ToLower() == "this" ||
                 model.OrganiserLastName.ToLower() == "user")
                model.OrganiserLastName = user.LastName;

            var parkEvent = _mapper.Map<ParkEvent>(model);

            parkEvent.Park = await _parkRepository.GetParkByIdAsync(model.ParkId);
            parkEvent.User = user;

            _parkRepository.Add(parkEvent);

            user.OrganisedEvents.Add(parkEvent);

            if (!await _parkRepository.SaveChangesAsync()) return null;
                
            var result = _mapper.Map<ParkEventModel>(parkEvent);
            if (hideUsername) result.Username = null;
            return result;
        }
        public async Task<ParkEventModel> UpdateParkEventAsync(ParkEventModel model)
        {
            var existing = await _parkRepository.GetEventByIdAsync(model.EventId);
            if (existing == null) return null;

            _mapper.Map(model, existing);

            if (existing.Park == null || model.ParkId != existing.Park.ParkId)
            {
                existing.Park = await _parkRepository.GetParkByIdAsync(model.ParkId);
            }

            if (await _parkRepository.SaveChangesAsync())
            {
                var result = _mapper.Map<ParkEventModel>(existing);
                result.Username = null;
                return result;
            }

            return null;
        }
        public async Task<bool> DeleteParkEventAsync(ParkEventModel model)
        {
            var parkEvent = _mapper.Map<ParkEvent>(model);
            _parkRepository.Delete(parkEvent);

            return await _parkRepository.SaveChangesAsync();
        }
        public async Task<LocalParksUserModel> GetEventOwner(int eventId, string userName = null)
        {
            var result = await _parkRepository.GetEventByIdAsync(eventId);

            if (result.User == null) return null;

            if (string.IsNullOrWhiteSpace(userName) || result.User.UserName == userName)
                return _mapper.Map<LocalParksUserModel>(result.User);

            return null;
        }
    }
}

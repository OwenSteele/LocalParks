using AutoMapper;
using LocalParks.Core.Contracts;
using LocalParks.Core.Domain;
using LocalParks.Core.Models;
using LocalParks.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocalParks.Infrastructure.Services
{
    public class ParkEventsService : IParkEventsService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IMapper _mapper;
        private readonly IEncryptionService _encryptionService;
        public ParkEventsService(IParkRepository parkRepository, IMapper mapper, IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
            _parkRepository = parkRepository;
            _mapper = mapper;
        }
        public async Task<ParkEventModel[]> GetAllParkEventModelsAsync()
        {
            var results = await _parkRepository.GetAllEventsAsync();

            return _mapper.Map<ParkEventModel[]>(results);
        }
        public async Task<ParkEventModel[]> GetSearchedParkEventModelsAsync(
            string searchTerm = null,
            string parkId = null,
            DateTime? date = null)
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

            return _mapper.Map<ParkEventModel[]>(results); ;
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
                   where !onlyWithEvents || p.Events.Any(e => e.Date >= DateTime.Today)
                   select new SelectListItem
                   {
                       Selected = false,
                       Text = p.Name,
                       Value = _encryptionService.Encrypt(p.ParkId)
                   };
        }
        public async Task<ParkModel> GetParkAsync(int parkId)
        {
            var result = await _parkRepository.GetParkByIdAsync(parkId);

            if (result == null) return null;

            return _mapper.Map<ParkModel>(result);
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
            var parkEvent = await _parkRepository.GetEventByIdAsync(model.EventId);
            _parkRepository.Delete(parkEvent);

            return await _parkRepository.SaveChangesAsync();
        }
        public async Task<LocalParksUserModel> GetEventOwner(int eventId, string userName = null)
        {
            var result = await _parkRepository.GetEventByIdAsync(eventId);

            if (userName == null || result == null || result.User == null) return null;

            if (result?.User?.UserName == userName)
                return _mapper.Map<LocalParksUserModel>(result.User);

            return null;
        }

        public async Task<ParkEventModel[]> GetParkEventModelsByParkIdAsync(int parkId)
        {
            var results = await _parkRepository.GetEventsByParkIdAsync(parkId);

            return _mapper.Map<ParkEventModel[]>(results);
        }

        public ParkEventModel GetNewEvent()
        {
            return new ParkEventModel { Date = DateTime.Today };
        }
    }
}

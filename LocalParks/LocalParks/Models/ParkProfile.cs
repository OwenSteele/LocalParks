using AutoMapper;
using LocalParks.Core;

namespace LocalParks.Models
{
    public class ParkProfile : Profile
    {
        public ParkProfile()
        {
            CreateMap<Postcode, PostcodeModel>().ReverseMap();

            CreateMap<Park, ParkModel>().ReverseMap();

            CreateMap<SportsClub, SportsClubModel>()
                .ForMember(m => m.ParkId, o => o.MapFrom(c => c.Park.ParkId))
                .ForMember(m => m.ParkName, o => o.MapFrom(c => c.Park.Name))
                .ForMember(m => m.Sport, o => o.MapFrom(c => c.Sport.ToString()))
                .ReverseMap();

            CreateMap<ParkEvent, ParkEventModel>()
                .ForMember(m => m.ParkId, o => o.MapFrom(e => e.Park.ParkId))
                .ForMember(m => m.ParkName, o => o.MapFrom(e => e.Park.Name))
                .ReverseMap();

            CreateMap<Supervisor, SupervisorModel>()
                .ForMember(m => m.ParkId, o => o.MapFrom(s => s.Park.ParkId))
                .ForMember(m => m.ParkName, o => o.MapFrom(s => s.Park.Name))
                .ForMember(m => m.ParkPostcode, o => o.MapFrom(s => s.Park.Postcode))
                .ForMember(m => m.EmployeeId, o => o.MapFrom(s => s.SupervisorId))
                .ReverseMap();
        }
    }
}

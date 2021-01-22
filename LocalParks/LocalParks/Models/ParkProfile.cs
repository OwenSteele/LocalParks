using AutoMapper;
using LocalParks.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Models
{
    public class ParkProfile : Profile
    {
        public ParkProfile()
        {
            CreateMap<Park,ParkModel>().ReverseMap();
            CreateMap<SportsClub, SportsClubModel>().ReverseMap();
            CreateMap<Event, EventModel>().ReverseMap();
            CreateMap<Supervisor, SupervisorModel>().ReverseMap();
        }        
    }
}

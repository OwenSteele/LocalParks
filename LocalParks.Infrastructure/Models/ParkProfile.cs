﻿using AutoMapper;
using LocalParks.Core.Domain;
using LocalParks.Core.Domain.Shop;
using LocalParks.Core.Domain.User;
using LocalParks.Core.Models;
using LocalParks.Core.Models.Shop;
using System;

namespace LocalParks.Infrastructure.Models
{
    public class ParkProfile : Profile
    {
        public ParkProfile()
        {
            CreateMap<LocalParksUser, LocalParksUserModel>().ReverseMap();

            CreateMap<Postcode, PostcodeModel>().ReverseMap()
                .ForMember(p => p.Parks, o => o.Ignore());

            CreateMap<Park, ParkModel>()
                .ForMember(p => p.PostcodeNeighbourhood, o => o.MapFrom(p => p.Postcode.Neighbourhood))
                .ReverseMap()
                .ForMember(p => p.Supervisor, o => o.Ignore())
                .ForMember(p => p.SportClubs, o => o.Ignore())
                .ForMember(p => p.Events, o => o.Ignore());

            CreateMap<SportsClub, SportsClubModel>()
                .ForMember(m => m.ParkId, o => o.MapFrom(c => c.Park.ParkId))
                .ForMember(m => m.ParkName, o => o.MapFrom(c => c.Park.Name))
                .ForMember(m => m.Sport, o => o.MapFrom(c => c.Sport.ToString()))
                .ReverseMap()
                .ForMember(c => c.Sport, o => o.MapFrom(m => Enum.Parse(typeof(SportType), m.Sport)))
                .ForMember(p => p.Park, o => o.Ignore());

            CreateMap<ParkEvent, ParkEventModel>()
                .ForMember(m => m.ParkId, o => o.MapFrom(e => e.Park.ParkId))
                .ForMember(m => m.ParkName, o => o.MapFrom(e => e.Park.Name))
                .ForMember(m => m.Username, o => o.MapFrom(e => e.User.UserName))
                .ReverseMap()
                .ForMember(e => e.ParkId, o => o.MapFrom(e => e.ParkId))
                .ForMember(p => p.Park, o => o.Ignore())
                .ForMember(p => p.User, o => o.Ignore());

            CreateMap<Supervisor, SupervisorModel>()
                .ForMember(m => m.ParkId, o => o.MapFrom(s => s.Park.ParkId))
                .ForMember(m => m.ParkName, o => o.MapFrom(s => s.Park.Name))
                .ForMember(m => m.ParkPostcode, o => o.MapFrom(s => s.Park.PostcodeZone))
                .ForMember(m => m.EmployeeId, o => o.MapFrom(s => s.SupervisorId))
                .ReverseMap()
                .ForMember(s => s.ParkRef, o => o.MapFrom(m => m.ParkId))
                .ForMember(s => s.Park, o => o.Ignore());

            CreateMap<Order, OrderModel>()
                .ForMember(m => m.Username, o => o.MapFrom(o => o.User.UserName))
                .ReverseMap();
            CreateMap<Product, ProductModel>()
                 .ForMember(m => m.Category, o => o.MapFrom(o => o.Category.ToString()))
                 .ReverseMap()
                 .ForMember(c => c.Category, o => o.MapFrom(m => Enum.Parse(typeof(ProductCategoryType), m.Category)));

            CreateMap<OrderItem, OrderItemModel>()
                .ForMember(m => m.ProductId, o => o.MapFrom(i => i.Product.ProductId))
                .ForMember(m => m.ProductName, o => o.MapFrom(i => i.Product.Name))
                .ForMember(m => m.ImageId, o => o.MapFrom(i => i.Product.ImageId))
                .ForMember(m => m.OrderId, o => o.MapFrom(i => i.Order.OrderId))
                .ReverseMap()
                .ForMember(i => i.Product, o => o.Ignore());
        }
    }
}

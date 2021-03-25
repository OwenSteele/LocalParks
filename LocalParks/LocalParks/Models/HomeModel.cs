﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LocalParks.Models
{
    public class HomeModel
    {
        public SportsClubModel NewestSportsClub { get; set; }
        public ParkEventModel RecentEvent { get; private set; }
        public int OpenParksCount { get; set; }

        public ICollection<ParkModel> ParksClosingSoon { get; set; }
        public ICollection<ParkEventModel> EventsThisMonth { get; set; }
        public int ParkCount { get; }
        public ParkModel SelectedPark { get; set; }
        public string ClosestEmergencyNumber { get; set; }

        public HomeModel(ParkModel selectedPark,
            int parkCount,
            int openParksCount,
            ParkModel[] parksClosingSoon,
            ParkEventModel recentEvent,
            ParkEventModel[] upcomingEvents,
            SportsClubModel newestSportsClub)
        {
            NewestSportsClub = newestSportsClub;

            RecentEvent = recentEvent;

            EventsThisMonth = upcomingEvents;

            ParkCount = parkCount;

            OpenParksCount = openParksCount;

            ParksClosingSoon = parksClosingSoon;

            SelectedPark = selectedPark;
        }
    }
}

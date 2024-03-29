﻿using System;

namespace LocalParks.Core.Domain
{
    public class Supervisor
    {
        public int SupervisorId { get; set; }
        public Park Park { get; set; }
        public int ParkRef { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public DateTime StartingDate { get; set; }
        public string Office { get; set; }
        public string EmergencyNumber { get; set; }
    }
}
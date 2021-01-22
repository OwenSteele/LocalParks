using System;

namespace LocalParks.Core
{
    public class Supervisor
    {
        public int SupervisorId { get; set; }
        public Park Park { get; set; }
        public int ParkRef { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Salary { get; set; }
        public DateTime StartingDate { get; set; }
    }
}
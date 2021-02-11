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
        //decimal
        public double Salary { get; set; }
        public DateTime StartingDate { get; set; }
        public string Office { get; set; }
        public string EmergencyNumber { get; set; }
    }
}

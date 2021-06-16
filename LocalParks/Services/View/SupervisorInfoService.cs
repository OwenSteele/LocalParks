using LocalParks.Contracts;
using System;

namespace LocalParks.Services.View
{
    public class SupervisorInfoService : ISupervisorInfoService
    {
        public string Tenure(DateTime startingDate)
        {
            if (startingDate == DateTime.MinValue) return "N/A";

            var days = Math.Floor((DateTime.Now - startingDate).TotalDays);
            if (days < 1) return "Less than a day";
            if (days == 1) return "A day";

            var months = Math.Floor(days / (365.25 / 12));
            if (months < 1) return $"{days} days";
            if (months == 1) return "A month";

            var years = Math.Floor(days / 365.25);
            if (years < 1) return $"{months} months";
            if (years == 1) return "A year";

            return $"{years} years";
        }
    }
}

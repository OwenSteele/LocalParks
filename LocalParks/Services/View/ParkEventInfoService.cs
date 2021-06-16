using LocalParks.Contracts;
using System;
using System.Text;

namespace LocalParks.Services.View
{
    public class ParkEventInfoService : IParkEventInfoService
    {
        public int InDays(DateTime date) => (date - DateTime.Today).Days;

        public string ObsfucatedNumber(string number)
        {
            int len = number.Length;

            return new StringBuilder(
                new string('*', len - 3), len).Append(number[(len - 3)..]).ToString();
        }
    }
}

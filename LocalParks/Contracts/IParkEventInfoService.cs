using System;

namespace LocalParks.Contracts
{
    public interface IParkEventInfoService
    {
        public int InDays(DateTime date);
        public string ObsfucatedNumber(string number);
    }
}

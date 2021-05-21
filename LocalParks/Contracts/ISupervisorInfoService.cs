using System;

namespace LocalParks.Contracts
{
    public interface ISupervisorInfoService
    {
        public string Tenure(DateTime startingDate);
    }
}

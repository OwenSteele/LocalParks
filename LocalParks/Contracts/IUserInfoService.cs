using System;

namespace LocalParks.Contracts
{
    public interface IUserInfoService
    {
        public string GetMemberShipLength(DateTime membersince);
    }
}

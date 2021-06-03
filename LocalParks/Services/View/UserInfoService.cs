using LocalParks.Contracts;
using System;
using System.Text;

namespace LocalParks.Services.View
{
    public class UserInfoService : IUserInfoService
    {
        public string GetMemberShipLength(DateTime membersince)
        {
            if (membersince == DateTime.MinValue) return "N/A";

            var days = Math.Floor((DateTime.Now - membersince).TotalDays);
            if (days < 1) return "Less than a day";

            var result = new StringBuilder();

            var years = Math.Floor(days / 365.25);
            if (years > 1) result.Append($"{years} years");
            if (years == 1) result.Append($"{years} year");

            var months = Math.Floor((days - (years * 365.25)) / (365.25 / 12));
            if (months > 1) result.Append($"{months} months");
            if (months == 1) result.Append($"{months} month");

            days = Math.Floor((days - (years * 365.25))) - (months * (365.25 / 12));
            if (days > 1) result.Append($"{days} days");
            if (days == 1) result.Append($"{days} day");

            return result.ToString();
        }
    }
}

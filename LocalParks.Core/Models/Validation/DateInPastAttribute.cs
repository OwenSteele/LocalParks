using System;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Core.Models.Validation
{
    public class DateInPast : ValidationAttribute
    {
        private readonly Func<DateTime> _dateTimeNowProvider;

        public DateInPast()
          : this(() => DateTime.Now)
        {
        }

        public DateInPast(Func<DateTime> dateTimeNowProvider)
        {
            _dateTimeNowProvider = dateTimeNowProvider;
            ErrorMessage = "Date must be in the past";
        }

        public override bool IsValid(object value)
        {
            bool isValid = false;

            if (value is DateTime dateTime)
            {
                isValid = dateTime < _dateTimeNowProvider();
            }

            return isValid;
        }
    }
}

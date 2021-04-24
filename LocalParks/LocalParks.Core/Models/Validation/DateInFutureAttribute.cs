using System;
using System.ComponentModel.DataAnnotations;

namespace LocalParks.Core.Models.Validation
{
    public class DateInFuture : ValidationAttribute
    {
        private readonly Func<DateTime> _dateTimeNowProvider;

        public DateInFuture()
          : this(() => DateTime.Now)
        {
        }

        public DateInFuture(Func<DateTime> dateTimeNowProvider)
        {
            _dateTimeNowProvider = dateTimeNowProvider;
            ErrorMessage = "Date must be in the future";
        }

        public override bool IsValid(object value)
        {
            bool isValid = false;

            if (value is DateTime dateTime)
            {
                isValid = dateTime > _dateTimeNowProvider();
            }

            return isValid;
        }
    }
}

using LocalParks.Models.Validation;
using System;
using Xunit;

namespace LocalParks.Tests.Validation
{
    public class DateInFutureAttributeTests
    {
        [InlineData(false, -1)]
        [InlineData(false, 0)]
        [InlineData(true, 1)]
        [Theory]
        public void ShouldValidateDateIsInTheFuture(bool expectedIsValid, int secondsToAdd)
        {
            var dateTimeNow = new DateTime(2020, 1, 28);

            var attribute = new DateInFuture(() => dateTimeNow);

            var isValid = attribute.IsValid(dateTimeNow.AddSeconds(secondsToAdd));

            Assert.Equal(expectedIsValid, isValid);
        }

        [Fact]
        public void ShouldHaveExpectedErrorMessage()
        {
            var attribute = new DateInFuture();

            Assert.Equal("Date must be in the future", attribute.ErrorMessage);
        }
    }
}

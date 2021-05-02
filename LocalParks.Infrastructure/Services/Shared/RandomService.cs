using LocalParks.Core.Contracts.Shared;
using System;

namespace LocalParks.Infrastructure.Services.Shared
{
    public class RandomService : IRandomService
    {
        private readonly Random _random;
        private readonly bool _seeded;

        public RandomService()
        {
            if (!_seeded)
            {
                _random = new();
                _seeded = true;
            }
        }

        public int Generate(int max, int min = 0)
        {
            return _random.Next(min, max);
        }
    }
}

﻿namespace LocalParks.Core.Chart
{
    public class XAxes
    {
        public string Id { get; set; }
        public bool Display { get; set; } = true;
        public string Type { get; set; }
        public Ticks Ticks { get; set; }
    }
}
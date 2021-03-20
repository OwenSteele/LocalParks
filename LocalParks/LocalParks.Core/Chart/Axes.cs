using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalParks.Core.Chart
{
    public class Axes
    {        
        public string Id { get; set; }
        public bool Display { get; set; }
        public string Type { get; set; }
        public Ticks Ticks { get; set; }
        public ScaleLabel ScaleLabel { get; set; }
    }
}

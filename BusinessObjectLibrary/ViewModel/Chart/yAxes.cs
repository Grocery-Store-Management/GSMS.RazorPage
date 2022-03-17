using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjectLibrary.ViewModel.Chart
{
    public class yAxes
    {
        public string id { get; set; }
        public bool display { get; set; }
        public string type { get; set; }
        public Ticks ticks { get; set; }
    }
}

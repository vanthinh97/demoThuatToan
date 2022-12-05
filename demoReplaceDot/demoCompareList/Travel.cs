using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoCompareList
{
    public class Travel
    {
        public int Id { get; set; }
        public List<Car> Cars {get; set; }
    }

    public class Car
    {
        public int Id { get; set; }
        public List<int> Codes { get; set; }
    }
}

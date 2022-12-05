using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demoTrimSpace
{
    public class Car
    {
        public int Id { get; set; }
        public List<CarList> CarLists { get; set; }
    }

    public class CarList
    {
        public int Id2 { get; set; }
        public string Door { get; set; }
    }
}

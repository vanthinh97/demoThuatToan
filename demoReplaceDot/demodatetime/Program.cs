using System;
using System.Collections.Generic;
using System.Linq;

namespace demodatetime
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var time = DateTime.Now;
            var gio = time.Hour * 60;
            var phut = time.Minute;

            Console.WriteLine("gio: " + gio + '\n' + "phut: " + phut);
            Console.ReadLine();
            Console.WriteLine("----------------------");

            var date1 = new DateTime(2008, 5, 1, 8, 30, 52);
            Console.WriteLine(date1);

            var startDate = new List<DateTime?> { time, date1 }.Max();

            Console.WriteLine(startDate);
        }
    }
}

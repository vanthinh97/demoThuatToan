using System;
using System.Collections.Generic;
using System.Linq;

namespace demoReplaceDot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string s = "abca ... bcabcab ... cabc";
            var foundIndexes = new List<int>();

            s = s.Replace("...", "_(R)_");
            //Console.WriteLine(s);

            for (int i = s.IndexOf("(R)"); i > -1; i = s.IndexOf("(R)", i + 1))
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
            }
            //text.Replace(String.Format("~{0}~", searchValue), "~");

            //foreach (var item in foundIndexes)
            //{
            //    //s.Remove(item, 1).Insert(item, "5");

            //    Console.WriteLine(item);
            //    s = s.Remove(item, 3).Insert(item, $"_{item}_");
            //}

            for (int i = 0; i < foundIndexes.Count; i++)
            {
                s = s.Remove(foundIndexes[i] + 1, 1).Insert(foundIndexes[i] + 1, $"{i + 1}");
            }

            Console.WriteLine(s);   

            Console.ReadLine();


        }

    }
}

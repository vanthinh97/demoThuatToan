// See https://aka.ms/new-console-template for more information
using demoTrimSpace;
using System.Text;
using System.Text.RegularExpressions;

//Console.WriteLine("Hello, World!");

//Console.OutputEncoding = Encoding.UTF8;
//var sent = "   Lập Trình     Không Khó     ";
//sent = sent.Trim();
//var trimmer = new Regex(@"\s\s+");
//sent = trimmer.Replace(sent, " ");

//Console.WriteLine(sent);

var car = new Car
{
    Id = 0,
    //Codes = new List<int> { 1, 2}
    CarLists = new List<CarList>
    {
        new CarList
        {
            Id2 = 1,
            Door = "aaa"
        }
    }
};

var abc = car.CarLists.Where(x => x.Door != null)?.Select(x => x.Door)?.ToList();
Console.WriteLine(abc.Any() ? abc[0] : null );

//if (car.CarLists?.Any() != null)
//{
//    Console.WriteLine("This not null");
//}
//else
//{
//    Console.WriteLine("This null");
//}
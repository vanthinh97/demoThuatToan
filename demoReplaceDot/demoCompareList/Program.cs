// See https://aka.ms/new-console-template for more information
using demoCompareList;

Console.WriteLine("Hello, World!");


var list1 = new List<int> { 1, 2, 4};
var list2 = new List<int> { 1 };
var list3 = new List<int>();
Console.WriteLine("l3: " + list3.Count);

Console.WriteLine(list1.Except(list2).Any());

var abc = list1.Except(list2).ToList();

foreach (var item in abc)
{
    Console.WriteLine(item);
}

//var firstNotSecond = list1.Except(list2).ToList();

//foreach (var item in firstNotSecond)
//{
//    Console.WriteLine(item);
//}

//Console.WriteLine(firstNotSecond.Any());


//var abc = new List<KeyValuePair<int, string>>();

//Console.WriteLine(abc.ToList().Select(x => x.Key).Any());


//Travel a1 = new Travel
//{
//    Id = 1,
//    Cars = new List<Car>
//    {
//        new Car
//        {
//            Id = 1,
//            Codes = new List<int> { 1, 2, 3 }
//        }
//    }
//};

//Travel a2 = new Travel
//{
//    Id = 1,
//    Cars = new List<Car>
//    {
//        new Car
//        {
//            Id = 1,
//            Codes = new List<int> { 1, 2, 3 }
//        }
//    }
//};

//var abc = new List<Travel> { a1, a2 };

//var lstInt = abc.Select(x => x.Cars.Select(y => y.Id)).ToList();

//foreach (var item in lstInt)
//{
//    for (int i = 0; i < item.Count(); i++)
//    {
//        Console.WriteLine(i);
//    }
//}

//List<string> myList = new List<string> { "a", "b" , "c"};
//string combinedString = string.Join(", ", myList);
//Console.WriteLine(combinedString);

var abcdf = list2.Where(x => x == 6);
abcdf = abcdf.ToList();
Console.WriteLine(abcdf.Count());

List<KeyValuePair<int, DateTime>> anc = new List<KeyValuePair<int, DateTime>>()
{
    new KeyValuePair<int, DateTime>(0, new DateTime(2023, 04, 21)),
    new KeyValuePair<int, DateTime>(1, new DateTime(2023, 04, 21)),
};

var jjj = anc.Where(x => x.Value == DateTime.Now.Date).Count();
Console.WriteLine("so: " + jjj);




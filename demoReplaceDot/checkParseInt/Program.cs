// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

string x = "42";
if (int.TryParse(x, out int value))
{
    Console.WriteLine(value);
    Console.WriteLine(x);
}
else
{
    Console.WriteLine("abc");
    Console.WriteLine(x);
}

var listAnswer = new List<string> { "a", "b", "c", "d" };
var answerNotSuffer = "1,2,-3";

var answerNotShuffles = answerNotSuffer != null
                                ? answerNotSuffer.Split(",").ToList()
                                : new List<string>();

var isFalse = answerNotShuffles.Select(an => int.TryParse(an, out _)).Any(isNumber => !isNumber);
//var abc = answerNotShuffles.Select(an => int.Parse(an));
if (isFalse)
{
    Console.WriteLine("False1");
}
else if (answerNotShuffles.Select(an => int.Parse(an)).Where(x => x <= 0 || x > listAnswer.Count()).Any())
{
    Console.WriteLine("False2");
}

Console.ReadLine();


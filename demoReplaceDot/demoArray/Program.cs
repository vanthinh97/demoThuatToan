// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var abc = new List<string>();
var a = 1;
var b = 1;

abc.Add(a.ToString());
abc.Add(b.ToString());

var bc = abc.ToArray();

Console.WriteLine(bc);


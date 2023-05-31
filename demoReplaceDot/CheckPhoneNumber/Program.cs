// See https://aka.ms/new-console-template for more information
using CheckPhoneNumber;

Console.WriteLine("Hello, World!");


Console.WriteLine(PhoneNumber.IsPhoneNbr("q^&*^&jhkjdsv"));



var infoDupValue = "abc";
var info = "Abc ";
Console.WriteLine(string.Compare(infoDupValue, info.ToLower().Trim(), true));

List<KeyValuePair<int, string>> abc = new List<KeyValuePair<int, string>>();
if (!abc.Any())
{
    Console.WriteLine("abc");
}

string getBetween(string strSource, string strStart, string strEnd)
{
    if (strSource.Contains(strStart) && strSource.Contains(strEnd))
    {
        int Start, End;
        Start = strSource.IndexOf(strStart, 0) + strStart.Length;
        End = strSource.IndexOf(strEnd, Start);
        return strSource.Substring(Start, End - Start);
    }

    return "";
}

string source = "This is an example string and my data is here";
string data = getBetween(source, "m", "is");
Console.WriteLine(data);

var aaa = new Array[] {};
if (aaa.Any()) Console.WriteLine("true");

var ccc = "000653b";
if (int.TryParse(ccc, out _))
{
    Console.WriteLine("number");
}

long time = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

Console.WriteLine(time);



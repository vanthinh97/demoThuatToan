// See https://aka.ms/new-console-template for more information
using demoCalculateTimeElementAwait;
using System.Text.RegularExpressions;

//Console.WriteLine("Hello, World!");



var watch = new System.Diagnostics.Stopwatch();

watch.Start();

var moto = new Moto();

var a0 = await moto.AccessTheWebAsync();
var a1 = await moto.AccessTheWebAsync1();
var a2 = await moto.AccessTheWebAsync2();
var a3 = await moto.AccessTheWebAsync3();
//await Task.WhenAll(a0, a1, a2, a3);

//int b0 = await a0;
//int b1 = await a1;
//int b2 = await a2;
//int b3 = await a3;

int b0 = a0;
int b1 = a1;
int b2 = a2;
int b3 = a3;

watch.Stop();

Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

//// Create a string array with the lines of text
//string[] lines = { "First line", "Second line", "Third line" };

//// Set a variable to the Documents path.
//string docPath =
//"C:\\Users\\thinh\\OneDrive\\Desktop\\";

////C:\Users\thinh\OneDrive\Desktop
//// Write the string array to a new file named "WriteLines.txt".
//using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "dulieuemail.txt")))
//{
//    foreach (string line in lines)
//        outputFile.WriteLine(line);
//}

////file in disk
//var FileUrl1 = @"C:\Users\thinh\OneDrive\Desktop\dulieuemail1.txt";

////file lines
//string[] mang1 = File.ReadAllLines(FileUrl1);

////file in disk
//var FileUrl2 = @"C:\Users\thinh\OneDrive\Desktop\dulieuemail2.txt";

////file lines
//string[] mang2 = File.ReadAllLines(FileUrl2);

//var abc = mang2.Except(mang1);
//foreach (var item in abc)
//{
//    Console.WriteLine(item);
//}

var mailTemplate = "123@gmail42.com";
var abc = mailTemplate.Split("(?<=\\D)(?=\\d)");
Regex re = new Regex("(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
Match match = re.Match(mailTemplate);
var alpha = match.Groups["Alpha"].Value;
var num = match.Groups["Numeric"].Value;

var nnn = Regex.Split(mailTemplate, @"\D+");
Console.WriteLine(nnn);

var listMail = new List<string>();
for (int i = 0; i < 100; i++)
{
    mailTemplate = i + "thinh@gmail.com";
    listMail.Add(mailTemplate);
}

var listMailNotCheck = new List<string>();
var listCheckDup = new List<string>();
var countDup = 0;
foreach (var item in listMail)
{
    //var a = item.Split('@');
    //var mailName = a[0];
    //var subMail = mailName.Substring(4);

    //if (listCheckDup.Contains(subMail)) 
    //{
    //    listMailNotCheck.Add(item);
    //    countDup++;
    //    continue;
    //} 
    //listCheckDup.Add(subMail);

    //var abc = item.Split("(?<=\\D)(?=\\d)");

    //if (true)
    //{

    //}
    //if (listCheckDup.Contains(abc))
    //{
    //    listMailNotCheck.Add(item);
    //    countDup++;
    //    continue;
    //}
    //listCheckDup.Add(abc);
}
Console.ReadLine();



//foreach (var item in listMail)
//{
//    var a = item.Split('@');
//    var mailName = a[0];
//    if (mailName.Length >= sophantucatradechecktrung)
//    {
//        var subMailFirst = mailName.Remove(mailName.Length - 3, 3);
//        var subMailLast = mailName.Substring(4);
//        if (checkDupFirst == subMailFirst || checkDupLast == subMailLast)
//        {
//            countDup++;
//            continue;
//        }
//        checkDupFirst = subMailFirst;
//        checkDupLast = subMailLast;
//    }
//}

var emailspilt = "123";
var host = emailspilt.Split('@')?.Last();
Console.WriteLine(host);


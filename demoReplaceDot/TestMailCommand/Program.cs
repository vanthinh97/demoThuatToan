// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;
using TestMailCommand;


//var fromMail = "thinhdang.havi@gmail.com";
var regex = new Regex(@"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,})$");

var mailTo = "thinhdangvan1997@gmail.com";
var listMailTo = new List<string>();

//for (int i = 0; i < 100; i++)
//{
//    mailTo = "thinhtestsvd" + i + "@gmail.com";
//    listMailTo.Add(mailTo);
//}

//var listMail = new List<string> {
//    "phong1103v1@cls.vn",
//    "phong1103v2@cls.vn"
//};

var telnet = new MailChecker();


//file in disk
var FileUrl = @"C:\Users\thinh\OneDrive\Desktop\dulieuemail.txt";

//file lines
string[] lines = File.ReadAllLines(FileUrl);

var soMail = lines.Skip(2000).Take(1000).ToList();

var listBig = new List<List<string>>();
// Xử lý chia nhỏ số lượng mail để chạy đa luồng
const int sophantutronglist = 500;
int songuyen = soMail.Count / sophantutronglist;
int sodu = soMail.Count % sophantutronglist;
var sohang = sodu > 0 ? songuyen + 1 : songuyen;
//----------------------------------------------

var listMail = new List<string>();
for (int i = 0; i < sohang; i++)
{
    listMail = soMail.Skip(i * sophantutronglist).Take(sophantutronglist).ToList();
    listBig.Add(listMail);
}

var listResult = new List<string>();
var listMailNotExist = new List<string>();
foreach (var item in listBig)
{
    listMailNotExist = await telnet.TestRecipient(item, 1);
    listResult.AddRange(listMailNotExist);
    Thread.Sleep(10000);
}
//var listResult = await telnet.TestRecipient2(soMail);

var result = listResult;
//foreach (var item in result)
//{
//    Console.WriteLine(item);
//}

//var result = await telnet.TestRecipient(listMail, 1);
Console.WriteLine("Total: " + result.Count);

var abc = result.ToArray();
string docPath = "C:\\Users\\thinh\\OneDrive\\Desktop\\";
using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "1dulieuemail27.txt")))
{
    foreach (string line in abc)
        outputFile.WriteLine(line);
}

//var testTcp = new TcpClientTest();
//testTcp.CheckMail();

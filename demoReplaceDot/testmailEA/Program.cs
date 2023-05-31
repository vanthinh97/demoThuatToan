// See https://aka.ms/new-console-template for more information
using EASendMail;

Console.WriteLine("Hello, World!");

var userName = "THINHMAIL";
var senderEmail = "thinhdang.havi@gmail.com";
var passWord = "R0iZZFZS5IZv&";
var host = "14.225.5.136";
var port = 25;
var receipient = "thinhdv@huongvietgroup.com";

try
{
    SmtpMail oMail = new SmtpMail("TryIt");

    // Set sender email address, please change it to yours
    oMail.From = senderEmail;

    // Set recipient email address, please change it to yours
    oMail.To = receipient;

    // Do not set SMTP server address
    SmtpServer oServer = new SmtpServer("");

    Console.WriteLine("start to test email address ...");

    SmtpClient oSmtp = new SmtpClient();
    oSmtp.TestRecipients(oServer, oMail);

    Console.WriteLine("email address was verified!");
}
catch (Exception ep)
{
    Console.WriteLine("failed to test email with the following error:");
    Console.WriteLine(ep.Message);
}
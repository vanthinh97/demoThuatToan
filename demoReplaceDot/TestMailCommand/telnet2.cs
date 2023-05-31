using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMailCommand
{
    public partial class MailChecker
    {
        /// <summary>
        /// Test Recipient
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<List<string>> TestRecipient2(List<string> emailAddress)
        {
            Console.WriteLine(emailAddress.Count);
            var listEmailNotExist = new List<string>();
            var listEmailValid = new List<string>();
            var listEmailNotCheck = new List<string>();

            // Lọc ra list host
            var listHost = new List<string>();
            var listEmailCheck = new List<KeyValuePair<string, string>>();
            foreach (var item in emailAddress)
            {
                var a = item.Split('@');
                if (a.Length > 1)
                {
                    var host = a[1];
                    listEmailCheck.Add(new KeyValuePair<string, string>(a[1], item));
                    if (listHost.Contains(host)) continue;
                    listHost.Add(host);
                }
            }

            foreach (var host in listHost)
            {
                var checkMails = listEmailCheck.Where(x => x.Key == host);
                var emailHost = GetEmailHost(host);

                if (emailHost != null && !checkMails.Equals(default(KeyValuePair<string, string>)))
                {
                    var mails = checkMails.Select(x => x.Value).ToList();

                    //Telnet to the SMTP server with the domain is emailHost and the default port is 25
                    var telnet = new TelnetConnection(emailHost, 25);
                    //Say hi to open the connection
                    telnet.WriteLine("EHLO hi");
                    var result = string.Empty;
                    long startTimeMillisStep1 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    var statusStep1 = CheckTelnet(telnet, startTimeMillisStep1, 1, 0);

                    if (!string.IsNullOrEmpty(statusStep1))
                    {
                        //Check Mail From
                        telnet.WriteLine("MAIL FROM:<thinhdang.havi@gmail.com>");
                        long startTimeMillisStep2 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                        var statusStep2 = CheckTelnet(telnet, startTimeMillisStep2, 2, 0);
                        var abc = 1;
                        if (!string.IsNullOrEmpty(statusStep2))
                        {
                            if (statusStep2.Split(" ").FirstOrDefault() != "250")
                            {
                                Console.WriteLine("Mail from k hoạt động: " + String.Join(", ", mails));
                                listEmailNotCheck.AddRange(mails);
                            }
                            else
                            {
                                foreach (var mail in mails)
                                {
                                    Console.WriteLine("Da Check " + abc);
                                    abc++;
                                    long startTimeMillisStep3 = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                                    telnet.WriteLine($"RCPT TO:<{mail}>");
                                    result = CheckTelnet(telnet, startTimeMillisStep3, 3, 0);

                                    if (string.IsNullOrEmpty(result))
                                    {
                                        listEmailNotCheck.Add(mail);
                                    }

                                    if (result.Split(" ").FirstOrDefault() != "250")
                                    {
                                        listEmailNotExist.Add(mail);
                                        Console.WriteLine("So luong k ton tai: " + listEmailNotExist.Count());

                                        continue;
                                    }

                                    listEmailValid.Add(mail);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Mail chua check: " + String.Join(", ", mails));
                            listEmailNotCheck.AddRange(mails);
                        }
                    }

                    telnet.WriteLine("QUIT");
                }
                else
                {
                    var abc = listEmailCheck.Where(x => x.Key == host);
                    if (!checkMails.Equals(default(KeyValuePair<string, string>)))
                    {
                        listEmailNotExist.AddRange(abc.Select(x => x.Value));
                        Console.WriteLine(listEmailNotExist.Count());
                    }
                }

                Thread.Sleep(5000);
            }


            //var listTask = listHost.Select(host => Task.Run(() =>
            //{
                
            //}));

            //await Task.WhenAll(listTask);

            // Kiểm tra lại những mail chưa check
            var resultCheckAgain = new List<string>();
            var thoihan = DateTime.Now.AddMinutes(1);
            if (listEmailNotCheck.Count > 0)
            {
                //do
                //{
                //    emailAddress = listEmailNotCheck;
                //    result = await TestRecipient(emailAddress);
                //} while (DateTime.Now >= thoihan);

                emailAddress = listEmailNotCheck;
                resultCheckAgain = await TestRecipient2(emailAddress);
            }

            Console.WriteLine("So mail hợp lệ: " + listEmailValid.Count);
            listEmailNotExist.AddRange(resultCheckAgain);
            return listEmailNotExist;
        }
    } 
}

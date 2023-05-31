using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestMailCommand
{
    enum Verbs
    {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }

    enum Options
    {
        SGA = 3
    }

    public static class EmailStatus
    {
        // Email được bảo mật hoặc độ uy tín của email chưa đủ lâu
        // Sẽ không check những email có domain này
        public static string EmailProtect = "EmailProtect";
    }

    public static class OS
    {
        public static bool IsWin() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMac() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsGnu() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string GetCurrent()
        {
            return
            (IsWin() ? "win" : null) ??
            (IsMac() ? "mac" : null) ??
            (IsGnu() ? "gnu" : string.Empty);
        }
    }

    public class TelnetConnection
    {
        TcpClient tcpSocket;

        int TimeOutMs = 400;

        public TelnetConnection(string Hostname, int Port)
        {
            tcpSocket = new TcpClient(Hostname, Port);
        }

        public void WriteLine(string cmd)
        {
            Write(cmd + "\r\n");
        }

        public void Write(string cmd)
        {
            if (!tcpSocket.Connected) return;
            byte[] buf = System.Text.ASCIIEncoding.ASCII.GetBytes(cmd.Replace("xFF", "xFFxFF"));
            tcpSocket.GetStream().Write(buf, 0, buf.Length);
        }

        public string Read()
        {
            if (!tcpSocket.Connected) return string.Empty;
            StringBuilder sb = new StringBuilder();
            do
            {
                ParseTelnet(sb);
                if (tcpSocket.Available < 1)
                {
                    Thread.Sleep(TimeOutMs);
                }
            }
            while (tcpSocket.Available > 0);
            return sb.ToString();
        }

        public void Stop()
        {
            if (!tcpSocket.Connected) return;
            //var cmd = "QUIT";
            tcpSocket?.Close();
        }

        public bool IsConnected
        {
            get { return tcpSocket.Connected; }
        }

        void ParseTelnet(StringBuilder sb)
        {
            while (tcpSocket.Available > 0)
            {
                int input = tcpSocket.GetStream().ReadByte();
                switch (input)
                {
                    case -1:
                        break;
                    case (int)Verbs.IAC:
                        // interpret as command
                        int inputverb = tcpSocket.GetStream().ReadByte();
                        if (inputverb == -1) break;
                        switch (inputverb)
                        {
                            case (int)Verbs.IAC:
                                //literal IAC = 255 escaped, so append char 255 to string
                                sb.Append(inputverb);
                                break;
                            case (int)Verbs.DO:
                            case (int)Verbs.DONT:
                            case (int)Verbs.WILL:
                            case (int)Verbs.WONT:
                                // reply to all commands with "WONT", unless it is SGA (suppres go ahead)
                                int inputoption = tcpSocket.GetStream().ReadByte();
                                if (inputoption == -1) break;
                                tcpSocket.GetStream().WriteByte((byte)Verbs.IAC);
                                if (inputoption == (int)Options.SGA)
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WILL : (byte)Verbs.DO);
                                else
                                    tcpSocket.GetStream().WriteByte(inputverb == (int)Verbs.DO ? (byte)Verbs.WONT : (byte)Verbs.DONT);
                                tcpSocket.GetStream().WriteByte((byte)inputoption);
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        sb.Append((char)input);
                        break;
                }
            }
        }
    }

    public partial class MailChecker
    {
        /// <summary>
        /// Test Recipient
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<List<string>> TestRecipient(List<string> emailAddress, int numberCheckAgain)
        {
            Console.WriteLine("So email kiem tra: " + emailAddress.Count);
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
                    if (listHost.Contains(host) || host == "cls.vn") continue;
                    listHost.Add(host);
                }
            }

            foreach (var host in listHost)
            {
                var checkMails = listEmailCheck.Where(x => x.Key == host);
                Console.WriteLine("Host checking: " + host);

                var listBig = new List<List<string>>();
                var mails = checkMails.Select(x => x.Value).ToList();

                // Xử lý chia nhỏ số lượng mail để chạy đa luồng
                const int sophantutronglist = 50;
                int songuyen = mails.Count / sophantutronglist;
                int sodu = mails.Count % sophantutronglist;
                var sohang = sodu > 0 ? songuyen + 1 : songuyen;
                //----------------------------------------------

                var listMail = new List<string>();

                for (int i = 0; i < sohang; i++)
                {
                    listMail = mails.Skip(i * sophantutronglist).Take(sophantutronglist).ToList();

                    if (listMail.Count > 30)
                    {
                        var countDup = 1;
                        var checkDup = string.Empty;

                        var listEmailCharacter = new List<KeyValuePair<string, string>>();
                        Regex re = new Regex("(?<Alpha>[a-zA-Z]*)(?<Numeric>[0-9]*)");
                        foreach (var item in listMail)
                        {
                            Match match = re.Match(item);

                            var alpha = match.Groups["Alpha"].Value;
                            listEmailCharacter.Add(new KeyValuePair<string, string>(alpha, item));
                            if (checkDup == alpha)
                            {
                                countDup++;
                                continue;
                            }
                            checkDup = alpha;
                        }

                        Dictionary<Tuple<string>, int> keyValueCounts = listEmailCharacter
                            .GroupBy(x => Tuple.Create(x.Key))
                            .ToDictionary(g => g.Key, g => g.Count());

                        var abc = keyValueCounts.Where(x => x.Value > 30).Select(x => x.Key.Item1);
                        foreach (var item in keyValueCounts)
                        {
                            if (item.Value > 30)
                            {
                                var ann = listEmailCharacter.Where(x => x.Key == item.Key.Item1);
                                if (!ann.Equals(default(KeyValuePair<string, string>)) || ann.Any())
                                {
                                    var mailsBlock = ann.Select(x => x.Value);
                                    Console.WriteLine("So luong k ton tai: " + mailsBlock.Count());
                                    listEmailNotExist.AddRange(mailsBlock);
                                    listMail = listMail.Except(mailsBlock).ToList();
                                }
                            }
                        }
                    }

                    if (listMail.Any()) listBig.Add(listMail);
                }

                if (listBig.Any())
                {
                    Parallel.ForEach(listBig, mails =>
                    {
                        var emailHost = GetEmailHost(host);
                        if (emailHost != null && emailHost == EmailStatus.EmailProtect)
                        {
                            listEmailNotCheck.AddRange(mails);
                        }
                        else if (emailHost != null && CheckDomainValid(emailHost) && !checkMails.Equals(default(KeyValuePair<string, string>)))
                        {
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

                                            Console.WriteLine($"Da Check {abc} - thread {listBig.IndexOf(mails)} ");
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

                            try
                            {
                                telnet.WriteLine("QUIT");
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
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


                    });

                    Thread.Sleep(5000);
                }
            }
            //var listTask = listHost.Select(host => Task.Run(() =>
            //{
                
            //}));

            //await Task.WhenAll(listTask);

            // Kiểm tra lại những mail chưa check
            var result = new List<string>();
            //var thoihan = DateTime.Now.AddMinutes(1);
            //if (listEmailNotCheck.Count > 0 && numberCheckAgain < 2)
            //{
            //    //do
            //    //{
            //    //    emailAddress = listEmailNotCheck;
            //    //    result = await TestRecipient(emailAddress);
            //    //} while (DateTime.Now >= thoihan);

            //    emailAddress = listEmailNotCheck;
            //    numberCheckAgain++;
            //    result = await TestRecipient(emailAddress, numberCheckAgain);
            //}

            Console.WriteLine("So mail hợp lệ: " + listEmailValid.Count);
            listEmailNotExist.AddRange(result);
            return listEmailNotExist;
        }

        private string CheckTelnet(TelnetConnection telnet, long startTimeMillis, int step, int numberRetry)
        {
            var result = telnet.Read();
            if (string.IsNullOrEmpty(result))
            {
                if (numberRetry > 2)
                {
                    Console.WriteLine("TimeOutStep: " + step + "; retry " + numberRetry);
                    Thread.Sleep(5000);
                    return string.Empty; 
                }
                else
                {
                    numberRetry++;
                    return CheckTelnet(telnet, startTimeMillis, step, numberRetry);
                }    
            }
            return result;
        }

        /// <summary>
        /// Run the command on the Command Prompt
        /// </summary>
        /// <param name="command">The command</param>
        /// <returns>value returned from the Command Prompt after running the command</returns>
        private string RunCommand(string command)
        {
            try
            {
                var fileName = string.Empty;
                switch (OS.GetCurrent())
                {
                    case "win":
                        fileName = "cmd";
                        break;
                    case "mac":
                    case "gnu":
                        fileName = "/bin/bash";
                        break;
                }

                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(fileName, "/c " + command);
                // The following commands are needed to redirect the standard output. 
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();

                // Get the output into a string
                return proc.StandardOutput.ReadToEnd();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get the Email host of the emailAddress
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <returns>The Email Host of the emailAddress</returns>
        private string? GetEmailHost(string emailAddress)
        {
            //Get the domain from email address, it is the part after the @ character
            var host = emailAddress.Split('@')?.Last();

            if (!string.IsNullOrEmpty(host))
            {
                //Get all mx records of the domain, return value is a string
                var checkDNS = RunCommand("nslookup -q=mx " + host);
                if (checkDNS.Replace(" ", "").Split("mailexchanger=") is string[] splitArr)
                {
                    if (splitArr.Length > 2)
                    {
                        return splitArr[1].Split("\r").FirstOrDefault();
                    }

                    if (splitArr.Length == 2)
                    {
                        return EmailStatus.EmailProtect;
                    }
                }
            }
            Console.WriteLine("HostNotWork: " + host);
            Thread.Sleep(5000);
            return null;
        }

        /// <summary>
        /// Validate a domain name
        /// </summary>
        /// <param name="domain">domain name</param>
        /// <returns>returns true if it is a syntactically correct domain, otherwise returns false</returns>
        private bool CheckDomainValid(string domain)
        {
            Regex regex = new Regex("(?:[a-z0-9](?:[a-z0-9-]{0,61}[a-z0-9])?\\.)+[a-z0-9][a-z0-9-]{0,61}[a-z0-9]");
            return regex.IsMatch(domain);
        }
    }
}

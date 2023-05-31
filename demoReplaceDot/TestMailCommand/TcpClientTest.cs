using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestMailCommand
{
    public class TcpClientTest
    {

        /// <summary>
        /// Run the command on the Command Prompt
        /// </summary>
        /// <param name="command">The command</param>
        /// <returns>value returned from the Command Prompt after running the command</returns>
        private string RunCommand(string command)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

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
                    if (splitArr.Length > 1)
                    {
                        return splitArr[1].Split("\r").FirstOrDefault();
                    }
                }
            }
            Console.WriteLine("HostNotWork: " + host);
            return null;
        }



        public void CheckMail()
        {
            var mailTo = "thinhsadsad123@gmail.com";
            var emailHost = GetEmailHost("gmail.com");
            if (emailHost != null)
            {
                TcpClient tClient = new TcpClient(emailHost, 25);
                string CRLF = "\r\n";
                byte[] dataBuffer;
                string ResponseString;
                NetworkStream netStream = tClient.GetStream();
                StreamReader reader = new StreamReader(netStream);
                ResponseString = reader.ReadLine();
                /* Perform HELO to SMTP Server and get Response */
                dataBuffer = BytesFromString("HELO hi" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                dataBuffer = BytesFromString("MAIL FROM:<thinhdang.havi@gmail.com>" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                /* Read Response of the RCPT TO Message to know from google if it exist or not */
                dataBuffer = BytesFromString($"RCPT TO:<{mailTo}>" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                ResponseString = reader.ReadLine();
                if (GetResponseCode(ResponseString) == 550)
                {
                    Console.WriteLine("Mai Address Does not Exist !<br/><br/>");
                    Console.WriteLine("<B><font color='red'>Original Error from Smtp Server :</font></b>" + ResponseString);
                }
                /* QUITE CONNECTION */
                dataBuffer = BytesFromString("QUITE" + CRLF);
                netStream.Write(dataBuffer, 0, dataBuffer.Length);
                tClient.Close();
            }

        }

        private byte[] BytesFromString(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        private int GetResponseCode(string ResponseString)
        {
            return int.Parse(ResponseString.Substring(0, 3));
        }
    }
}

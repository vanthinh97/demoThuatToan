// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

Console.WriteLine("Hello, World!");

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
        (IsGnu() ? "gnu" : String.Empty);
    }
}


public class Response
{
    public int code { get; set; }
    public string stdout { get; set; }
    public string stderr { get; set; }
}

public enum Output
{
    Hidden,
    Internal,
    External
}

public static class Shell
{
    private static string GetFileName()
    {
        try
        {
            string fileName = "";
            switch (OS.GetCurrent())
            {
                case "win":
                    fileName = "cmd.exe";
                    break;
                case "mac":
                case "gnu":
                    fileName = "/bin/bash";
                    break;
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
    }


    private static string CommandConstructor(string cmd, Output? output = Output.Hidden, string dir = "")
    {
        try
        {
            if (!String.IsNullOrEmpty(dir))
            {
                //dir.Exists("");
            }
            switch (OS.GetCurrent())
            {
                case "win":
                    if (!String.IsNullOrEmpty(dir))
                    {
                        dir = $" \"{dir}\"";
                    }
                    if (output == Output.External)
                    {
                        cmd = $"{Directory.GetCurrentDirectory()}/cmd.win.bat \"{cmd}\"{dir}";
                    }
                    cmd = $"/c \"{cmd}\"";
                    break;
                case "mac":
                case "gnu":
                    if (!String.IsNullOrEmpty(dir))
                    {
                        dir = $" '{dir}'";
                    }
                    if (output == Output.External)
                    {
                        cmd = $"sh {Directory.GetCurrentDirectory()}/cmd.mac.sh '{cmd}'{dir}";
                    }
                    cmd = $"-c \"{cmd}\"";
                    break;
            }
            return cmd;
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
    }

    public static Response Term(string cmd, Output? output = Output.Hidden, string dir = "")
    {
        var result = new Response();
        var stderr = new StringBuilder();
        var stdout = new StringBuilder();
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = GetFileName();
            startInfo.Arguments = CommandConstructor(cmd, output, dir);
            startInfo.RedirectStandardOutput = !(output == Output.External);
            startInfo.RedirectStandardError = !(output == Output.External);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = !(output == Output.External);
            if (!String.IsNullOrEmpty(dir) && output != Output.External)
            {
                startInfo.WorkingDirectory = dir;
            }

            using (Process process = Process.Start(startInfo))
            {
                switch (output)
                {
                    case Output.Internal:
                        $"".fmNewLine();

                        while (!process.StandardOutput.EndOfStream)
                        {
                            string line = process.StandardOutput.ReadLine();
                            stdout.AppendLine(line);
                            Console.WriteLine(line);
                        }

                        while (!process.StandardError.EndOfStream)
                        {
                            string line = process.StandardError.ReadLine();
                            stderr.AppendLine(line);
                            Console.WriteLine(line);
                        }
                        break;
                    case Output.Hidden:
                        stdout.AppendLine(process.StandardOutput.ReadToEnd());
                        stderr.AppendLine(process.StandardError.ReadToEnd());
                        break;
                }
                process.WaitForExit();
                result.stdout = stdout.ToString();
                result.stderr = stderr.ToString();
                result.code = process.ExitCode;
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.Message);
        }
        return result;
    }
}
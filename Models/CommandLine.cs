using System;
using System.Diagnostics;

namespace DarkMode_2.Models;

public class CommandLine
{
    private string commandOutput = "";

    public string CommandOutput
    {
        get { return commandOutput; }
    }

    public int ExitCode { get; private set; }

    public CommandLine(string command)
    {
        var processInfo = new ProcessStartInfo("cmd.exe", $"/c \"{command}\"")
        {
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true
        };

        using (var process = new Process())
        {
            process.StartInfo = processInfo;
            process.OutputDataReceived += Process_OutputDataReceived;
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            ExitCode = process.ExitCode;
        }
    }

    private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.Data))
        {
            commandOutput += e.Data + "\n";
        }
    }
}
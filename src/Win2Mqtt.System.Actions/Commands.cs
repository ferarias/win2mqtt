using System.Diagnostics;

namespace Win2Mqtt.SystemActions.Windows
{
    public class CommandParameters
    {
        public required string CommandString { get; set; }
        public int WindowStyle { get; set; }
        public string ExecParameters { get; set; } = string.Empty;
        public string? MonitorId { get; set; }
    }

    public static class Commands
    {
        public static void RunCommand(CommandParameters commandParameters)
        {
            ProcessWindowStyle processWindowStyle = commandParameters.WindowStyle switch
            {
                0 => ProcessWindowStyle.Normal,
                1 => ProcessWindowStyle.Hidden,
                2 => ProcessWindowStyle.Minimized,
                3 => ProcessWindowStyle.Maximized,
                _ => ProcessWindowStyle.Normal,
            };

            var startInfo = new ProcessStartInfo(commandParameters.CommandString, commandParameters.ExecParameters)
            {
                WindowStyle = processWindowStyle

            };

            Process.Start(startInfo);
        }
    }
}

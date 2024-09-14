using System.ComponentModel;
using micmute_indicator.Helpers;

namespace micmute_indicator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ProcessHelper.CheckAlreadyRunning();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var commDeviceWatchdog = new CommDeviceWatchdog();
            var osd = new MuteForm();

            while (true)
            {
                if (commDeviceWatchdog.IsMuteWarningNeeded())
                {
                    osd.Display();
                }
                else
                {
                    osd.Hide();
                    osd.Hide();
                }
                Thread.Sleep(1000);
            }
        }
    }
}
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
            //var osd = new OnScreenDisplay();
            //var osd = new OSD();
            var osd = new MuteForm();

            while (true)
            {
                if (commDeviceWatchdog.IsMuteWarningNeeded())
                {
                    //osd.Show();
                    //osd.ShowDialog();
                    osd.Display();
                    //osd.Show();
                }
                else
                {
                    osd.Hide();
                    osd.Hide();
                }
                Thread.Sleep(1000);
            }

            //Application.Run(new Form1());
            //Application.Run();
        }
    }
}
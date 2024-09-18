using micmute_indicator.Helpers;

namespace micmute_indicator
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            ProcessHelper.CheckAlreadyRunning();
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            CommDeviceWatchdog commDeviceWatchdog = new();
            MuteForm osd = new();
            PeriodicTimer timer = new(TimeSpan.FromMilliseconds(1000));

            while (await timer.WaitForNextTickAsync())
            {
                if (commDeviceWatchdog.IsMuteWarningNeeded())
                {
                    osd.Display();
                }
                else
                {
                    osd.Hide();
                }
                GC.Collect(0);
            }
        }
    }
}
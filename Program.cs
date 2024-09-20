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
            Application.EnableVisualStyles();

            Application.Run(new Core());
        }
    }
}
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

            Application.ThreadException += (sender, e) => HandleUnhandledException(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => HandleUnhandledException(e.ExceptionObject as Exception);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();

            Application.Run(new Core());
        }

        private static void HandleUnhandledException(Exception? exception)
        {
            if (exception is null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Unhandled exception: {exception}");
            // Swallow the exception to prevent the application from crashing.
            // Transient audio-device/COM errors should not terminate the tray app.
        }
    }
}
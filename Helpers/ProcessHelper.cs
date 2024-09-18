using System.Diagnostics;

namespace micmute_indicator.Helpers
{
    internal class ProcessHelper
    {
        public static void CheckAlreadyRunning()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);

            if (processes.Length > 1)
            {
                foreach (Process process in processes)
                    if (process.Id != currentProcess.Id)
                        try
                        {
                            process.Kill();
                        }
                        catch (Exception)
                        {
                            Application.Exit();
                            return;
                        }
            }
        }

    }
}

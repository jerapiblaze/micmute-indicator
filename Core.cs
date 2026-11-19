using micmute_indicator.Helpers;

namespace micmute_indicator
{
    class Core : ApplicationContext
    {
        private static readonly MuteForm osd = new();
        private static readonly System.Windows.Forms.Timer primaryTimer = new();
        private static readonly NotifyIcon notifyIcon = new();
        private static bool showOSD = Properties.Settings.Default.ShowOSD;
        private static Icon? currentIcon;

        public Core()
        {
            InitalizeComponents();
            primaryTimer.Start();
        }

        private static void InitalizeComponents()
        {
            notifyIcon.Visible = false;
            notifyIcon.Text = "Mic-mute indicator";
            notifyIcon.MouseClick += NotifyIcon_OnClick;
            primaryTimer.Interval = 1000;
            primaryTimer.Tick += PrimaryTimer_Tick;
        }

        private static void NotifyIcon_OnClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                showOSD = !showOSD;
                Properties.Settings.Default.ShowOSD = showOSD;
                Properties.Settings.Default.Save();
                return;
            }
            AudioDeviceHelper.ToggleMute();
        }

        private static void SetNotifyIcon(Bitmap bitmap)
        {
            Icon? previousIcon = currentIcon;
            currentIcon = Icon.FromHandle(bitmap.GetHicon());
            notifyIcon.Icon = currentIcon;

            // The handle created by GetHicon() and wrapped by Icon.FromHandle is not owned/released
            // by the Icon object, so we must dispose it ourselves once it's no longer in use to avoid
            // leaking native GDI icon handles.
            previousIcon?.Dispose();
        }

        private static void WarnState_Check()
        {
            if (AudioDeviceHelper.CheckAllActive())
            {
                notifyIcon.Visible = true;
                if (AudioDeviceHelper.CheckAllMuted())
                {
                    SetNotifyIcon(Properties.Resources.icons8_mute);
                    notifyIcon.Text = "Muted";
                    if (showOSD)
                    {
                        osd.Display();
                    }
                    else
                    {
                        osd.Hide();
                    }
                }
                else
                {
                    SetNotifyIcon(Properties.Resources.icons8_unmute);
                    notifyIcon.Text = "Unmuted";
                    osd.Hide();
                }
            }
            else
            {
                notifyIcon.Visible = false;
                osd.Close();
            }
        }

        private static void PrimaryTimer_Tick(object? sender, EventArgs e)
        {
            try
            {
                WarnState_Check();
            }
            catch (Exception)
            {
                // Avoid crashing the tray app on transient audio/UI errors; the next tick will retry.
            }
        }
    }
}

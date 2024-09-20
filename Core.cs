using micmute_indicator.Helpers;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace micmute_indicator
{
    class Core : ApplicationContext
    {
        private static readonly MuteForm osd = new();
        private static readonly System.Windows.Forms.Timer primaryTimer = new();
        private static readonly NotifyIcon notifyIcon = new();
        private static bool showOSD = Properties.Settings.Default.ShowOSD;

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

        private static void WarnState_Check()
        {
            if (AudioDeviceHelper.CheckAllActive()) 
            { 
                notifyIcon.Visible = true;
                if (AudioDeviceHelper.CheckAllMuted())
                {
                    notifyIcon.Icon = Icon.FromHandle(Properties.Resources.icons8_mute.GetHicon());
                    notifyIcon.Text = "Muted";
                    if (showOSD)
                    {
                        osd.Display();
                    } else
                    {
                        osd.Hide();
                    }
                }
                else
                {
                    notifyIcon.Icon = Icon.FromHandle(Properties.Resources.icons8_unmute.GetHicon());
                    notifyIcon.Text = "Unmuted";
                    osd.Hide();
                }
            } else
            {
                notifyIcon.Visible = false;
                osd.Close();
            }
        }

        private static void PrimaryTimer_Tick(object? sender, EventArgs e)
        {
            WarnState_Check();
            GC.Collect(0);
        }
    }
}

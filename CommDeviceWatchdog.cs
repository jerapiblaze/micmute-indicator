using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace micmute_indicator
{
    internal class CommDeviceWatchdog
    {
        public MMDevice commDevice;
        
        public CommDeviceWatchdog(MMDevice commDevice) 
        {
            this.commDevice = commDevice;
        }

        public CommDeviceWatchdog()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            this.commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
        }

        public bool IsMuteWarningNeeded()
        {
            var isMuted = AudioDeviceHelper.CheckMutedState(this.commDevice);
            var hasActiveSession = AudioDeviceHelper.CheckActiveSession(this.commDevice);
            return isMuted && hasActiveSession;
        }

        private static void Watch_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker s = sender as BackgroundWorker;

            // Task
        }
    }
}

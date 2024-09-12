using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace micmute_indicator
{
    internal class AudioDeviceHelper
    {

        public static MMDevice GetDefaultCommDevice()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            var commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            return commDevice;
        }

        public static bool CheckActiveSession(MMDevice device)
        {
            SessionCollection sessionList = device.AudioSessionManager.Sessions;

            for (int i = 0; i < sessionList.Count; i++)
            {
                if (sessionList[i].State == AudioSessionState.AudioSessionStateActive)
                {
                    return true;
                }
            }

            return false;

        }

        public static bool CheckMutedState(MMDevice device)
        {
            bool status = device.AudioEndpointVolume.Mute;

            return status;

        }
    }
}
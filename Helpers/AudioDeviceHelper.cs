using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace micmute_indicator.Helpers
{
    internal class AudioDeviceHelper
    {

        public static MMDevice GetDefaultCommDevice()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                deviceEnumerator.Dispose();
                return commDevice;
            }
        }

        public static MMDevice GetDefaultConsoleDevice()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var consoleDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                deviceEnumerator.Dispose();
                return consoleDevice;
            }
        }

        public static MMDevice GetDefaultMUltimediaDevice()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var multimediaDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
                deviceEnumerator.Dispose();
                return multimediaDevice;
            }
        }

        public static bool CheckActiveSession(MMDevice device)
        {
            SessionCollection sessionList = device.AudioSessionManager.Sessions;
            device.AudioSessionManager.RefreshSessions();

            for (int i = 0; i < sessionList.Count; i++)
            {
                var session = sessionList[i];
                //MessageBox.Show(i + "\t" + session.DisplayName.ToString() + "\t" + session.State.ToString(), device.ToString() + "\t" + sessionList.Count);
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

        public static bool CheckAllMuted()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                var consoleDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                var multimediaDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);

                var commMuted = commDevice.AudioEndpointVolume.Mute;
                var consoleMuted = consoleDevice.AudioEndpointVolume.Mute;
                var multimediaMuted = multimediaDevice.AudioEndpointVolume.Mute;

                deviceEnumerator.Dispose();

                return commMuted && consoleMuted && multimediaMuted;
            }
        }

        public static bool CheckAllActive()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                var consoleDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                var multimediaDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);

                var commActive = CheckActiveSession(commDevice);
                var consoleActive = CheckActiveSession(consoleDevice);
                var multimediaActive = CheckActiveSession(multimediaDevice);

                deviceEnumerator.Dispose();

                return commActive || consoleActive && multimediaActive;
            }
        }

        public static void ToggleMute()
        {
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                var commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                var consoleDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                var multimediaDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);

                bool status = !commDevice.AudioEndpointVolume.Mute;

                commDevice.AudioEndpointVolume.Mute = status;
                consoleDevice.AudioEndpointVolume.Mute = status;
                multimediaDevice.AudioEndpointVolume.Mute = status;

                deviceEnumerator.Dispose();
            }
        }
    }
}
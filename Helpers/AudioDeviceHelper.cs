using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace micmute_indicator.Helpers
{
    internal class AudioDeviceHelper
    {

        public static MMDevice? GetDefaultCommDevice()
        {
            return TryGetDevice(Role.Communications);
        }

        public static MMDevice? GetDefaultConsoleDevice()
        {
            return TryGetDevice(Role.Console);
        }

        public static MMDevice? GetDefaultMUltimediaDevice()
        {
            return TryGetDevice(Role.Multimedia);
        }

        private static MMDevice? TryGetDevice(Role role)
        {
            try
            {
                using var deviceEnumerator = new MMDeviceEnumerator();
                return deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, role);
            }
            catch (Exception)
            {
                // No capture device available, or device temporarily unavailable (e.g. being reconfigured).
                return null;
            }
        }

        private static bool TryGetAllCaptureDevices(MMDeviceEnumerator deviceEnumerator, out MMDevice? commDevice, out MMDevice? consoleDevice, out MMDevice? multimediaDevice)
        {
            commDevice = null;
            consoleDevice = null;
            multimediaDevice = null;

            try
            {
                commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                consoleDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                multimediaDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckActiveSession(MMDevice? device)
        {
            if (device is null)
            {
                return false;
            }

            try
            {
                SessionCollection sessionList = device.AudioSessionManager.Sessions;
                device.AudioSessionManager.RefreshSessions();

                for (int i = 0; i < sessionList.Count; i++)
                {
                    if (sessionList[i].State == AudioSessionState.AudioSessionStateActive)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckMutedState(MMDevice? device)
        {
            try
            {
                return device?.AudioEndpointVolume.Mute ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckAllMuted()
        {
            try
            {
                using var deviceEnumerator = new MMDeviceEnumerator();
                if (!TryGetAllCaptureDevices(deviceEnumerator, out var commDevice, out var consoleDevice, out var multimediaDevice))
                {
                    return false;
                }

                var commMuted = commDevice!.AudioEndpointVolume.Mute;
                var consoleMuted = consoleDevice!.AudioEndpointVolume.Mute;
                var multimediaMuted = multimediaDevice!.AudioEndpointVolume.Mute;

                return commMuted && consoleMuted && multimediaMuted;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool CheckAllActive()
        {
            try
            {
                using var deviceEnumerator = new MMDeviceEnumerator();
                if (!TryGetAllCaptureDevices(deviceEnumerator, out var commDevice, out var consoleDevice, out var multimediaDevice))
                {
                    return false;
                }

                var commActive = CheckActiveSession(commDevice);
                var consoleActive = CheckActiveSession(consoleDevice);
                var multimediaActive = CheckActiveSession(multimediaDevice);

                return commActive || consoleActive && multimediaActive;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void ToggleMute()
        {
            try
            {
                using var deviceEnumerator = new MMDeviceEnumerator();
                if (!TryGetAllCaptureDevices(deviceEnumerator, out var commDevice, out var consoleDevice, out var multimediaDevice))
                {
                    return;
                }

                bool status = !commDevice!.AudioEndpointVolume.Mute;

                commDevice.AudioEndpointVolume.Mute = status;
                consoleDevice!.AudioEndpointVolume.Mute = status;
                multimediaDevice!.AudioEndpointVolume.Mute = status;
            }
            catch (Exception)
            {
                // Ignore transient audio device errors (e.g. device disconnected mid-toggle).
            }
        }
    }
}
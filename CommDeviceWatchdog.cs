using NAudio.CoreAudioApi;

namespace micmute_indicator
{
    internal class CommDeviceWatchdog
    {
        private MMDevice? device;
        private readonly bool deviceDetection;

        public CommDeviceWatchdog(MMDevice device)
        {
            this.device = device;
            this.deviceDetection = false;
        }

        public CommDeviceWatchdog()
        {
            this.device = null;
            this.deviceDetection = true;
        }

        public bool IsMuteWarningNeeded()
        {
            if (this.deviceDetection)
            {

                using (var deviceEnumerator = new MMDeviceEnumerator())
                {
                    var commDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                    var consoleDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Console);
                    var multimediaDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Multimedia);

                    var commIsMuted = AudioDeviceHelper.CheckMutedState(commDevice);
                    var commHasActiveSession = AudioDeviceHelper.CheckActiveSession(commDevice);
                    var consoleIsMuted = AudioDeviceHelper.CheckMutedState(consoleDevice);
                    var consoleHasActiveSession = AudioDeviceHelper.CheckActiveSession(consoleDevice);
                    var multimediaIsMuted = AudioDeviceHelper.CheckMutedState(multimediaDevice);
                    var multimediaHasActiveSession = AudioDeviceHelper.CheckActiveSession(multimediaDevice);

                    //commDevice.Dispose();
                    //consoleDevice.Dispose();
                    //multimediaDevice.Dispose();

                    return (commIsMuted && commHasActiveSession) || (consoleIsMuted && consoleHasActiveSession) || (multimediaIsMuted && multimediaHasActiveSession);
                }
            }
            else
            {
                if (this.device == null)
                {
                    return false;
                }

                var isMuted = AudioDeviceHelper.CheckMutedState(this.device);
                var hasActiveSession = AudioDeviceHelper.CheckActiveSession(this.device);

                return isMuted && hasActiveSession;
            }
        }

        public MMDevice? GetCurrentDevice()
        {
            return this.device;
        }
    }
}

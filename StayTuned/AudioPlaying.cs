using System;
using System.Runtime.InteropServices;
using CoreAudioApi;

namespace AudioDetector
{
    class AudioTest
    {

        public static bool IsAudioPlaying()
        {
            MMDeviceEnumerator deviceEnumerator = new MMDeviceEnumerator();
            MMDevice device = deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eConsole);
            
            return device.AudioMeterInformation.MasterPeakValue > 0;
        }


    }
}

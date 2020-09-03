using AudioDetector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput.Native;
using WindowsInput;
using CoreAudioApi;

namespace StayTuned
{
    public static class Program
    {
        static void Main(string[] args)
        {  
            Process selectedProcess = SelectProcess(AllProcesses());

            Console.WriteLine(selectedProcess.ProcessName);
            Console.WriteLine(AudioTest.IsAudioPlaying());
            chekc(selectedProcess);

            while (true)
            {
                Thread.Sleep(5000);
                if (!AudioTest.IsAudioPlaying())
                {
                    IntPtr prevHandle = GetForegroundWindow();
                    IntPtr windowHandle = selectedProcess.MainWindowHandle;
                    SetForegroundWindow(windowHandle);

                }
            }
        
       
        }

        //static SimpleAudioVolume chekc(Process id)
      static void chekc(Process id)

        {
            var DevEnum = new MMDeviceEnumerator();
            MMDevice device = DevEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            for (int i = 0; i < device.AudioSessionManager.Sessions.Count; i++)
            {
                AudioSessionControl session = device.AudioSessionManager.Sessions[i];
                Console.WriteLine(Process.GetProcessById(Convert.ToInt32(session.ProcessID)).ProcessName);
                if (Process.GetProcessById(Convert.ToInt32(session.ProcessID)).ProcessName.ToLower() == id.ProcessName.ToLower())
                {
                    Console.WriteLine("it exist");
                    Console.WriteLine(session.SimpleAudioVolume);
                }
            }
  
        }

        public static Process SelectProcess(List<Process> Proc)
        {
              
            for (int i = 0; i < Proc.Count; i++)
            {
                Process p = Proc[i];
                Console.WriteLine(i + ": " + p.ProcessName);
            }
            Console.WriteLine("\nWhich process would you like to StayTuned?");

            return Proc[Convert.ToInt32(Console.ReadLine())];
        }

        public static List<Process> AllProcesses()
        {
            List<Process> processlist = new List<Process>();

            var currentProcess = Process.GetProcesses();
            foreach (Process Proc in currentProcess.Where(p => p.MainWindowHandle != IntPtr.Zero))
            {
                processlist.Add(Proc);
            }
            return processlist;
        }
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
    }

}

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Handoff
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);
            if (args.Length != 0 && args[0].Contains("--setup"))
            {
                if(args[0] != "--setup-admin") Elevate();
                string DocuPath = HandoffToken.GetOrDefault();
                string FdPath = DocuPath + "\\InstallationPath.handoff";
                StreamWriter a = File.CreateText(FdPath);
                a.Write(DocuPath+"\\Freedeck");
                a.Close();
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("freedeck")) 
                {
                    if (key != null)
                    {
                        key.SetValue("", "Freedeck Handoff");
                        key.SetValue("URL Protocol", "");
                    }
                }
                using (RegistryKey key = Registry.ClassesRoot.CreateSubKey("freedeck\\shell\\open\\command"))
                {
                    if (key != null)
                    {
                        key.SetValue("", System.IO.Directory.GetCurrentDirectory()+"\\handoff.exe %1");
                    }
                }
            } else
            {
                string action = args[0].Split('/')[2];
                string appid = args[0].Split('/')[3];
                switch (action)
                {
                    case "download":
                        Downloader.Download(args[0].Split('/'));
                        break;
                    default:
                        Console.WriteLine("This doesn't exist.. sorry!");
                        break;
                }
            }
        }
        
        private static void Elevate()
        {
            var SelfProc = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = Environment.CurrentDirectory,
                FileName = "handoff.exe",
                Arguments = "--setup-admin",
                Verb = "runas"
            };
            try
            {
                Process.Start(SelfProc);
            }
            catch
            {
                Console.WriteLine("Unable to elevate!");
            }
        }
    }
}

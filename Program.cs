using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;

namespace Handoff
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                        Console.WriteLine("Finding download sources..");
                        Downloader.Download(args[0].Split('/'), Wc_finished);
                        break;
                    default:
                        Console.WriteLine("This doesn't exist.. sorry!");
                        break;
                }
            }
            Console.WriteLine("Press any key to close Handoff");
            Console.ReadLine();
        }

        static string Get(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private static void Wc_finished(object sender, AsyncCompletedEventArgs e)
        {
            string token = HandoffToken.GetToken();
            string url = "http://localhost:5754/handoff/" + token + "/reload-plugins";
            Get(url);
            Console.WriteLine("Asked server to reload plugins!");
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

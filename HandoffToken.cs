using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Handoff
{
    internal class HandoffToken
    {
        public static string GetOrDefault()
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Freedeck";
            if (File.Exists(path + "\\InstallationPath.handoff"))
            {
                return File.ReadAllText(path + "\\InstallationPath.handoff");
            }
            else return path;
        }
        public static string GetToken()
        {
            string DocuPath = GetOrDefault();
            string FdTPath = DocuPath + "\\token.handoff";
            string FdGTPath = DocuPath + "\\time.handoff";

            WebClient wc = new WebClient();
            var s = wc.DownloadData("http://localhost:5754/handoff/get-token");
            var token = Encoding.Default.GetString(s);

            if (!File.Exists(FdTPath) || !File.Exists(FdGTPath)) {
                StreamWriter a =File.CreateText(FdTPath);
                a.Write(token);
                a.Close();
                StreamWriter b = File.CreateText(FdGTPath);
                b.Write(DateTime.Now);
                b.Close();
            }
            return token;
        }

        private static void Wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            Console.WriteLine("");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Handoff
{
    internal class Downloader
    {
        public static void Download(string[] arggies, AsyncCompletedEventHandler finished)
        {
            string DocuPath = HandoffToken.GetOrDefault();
            string FdPath = DocuPath + "\\freedeck\\plugins\\";
            string url = arggies[4];
            url = Uri.UnescapeDataString(url);
            string to = FdPath + arggies[3] + ".Freedeck";
            url = Uri.UnescapeDataString(url);
            Console.WriteLine("Downloading plugin ID " + arggies[3]);
            WebClient wc = new WebClient();
            wc.DownloadFileTaskAsync(url, to);
            wc.DownloadFileCompleted += finished;
        }
    }
}

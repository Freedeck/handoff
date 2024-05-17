using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Handoff
{
    internal class Downloader
    {
        public static void Download(string[] arggies)
        {
            string DocuPath = HandoffToken.GetOrDefault();
            string FdPath = DocuPath + "\\freedeck\\plugins\\";
            string url = arggies[4];
            url = Uri.UnescapeDataString(url);
            string to = FdPath + arggies[3] + ".Freedeck";
            url = Uri.UnescapeDataString(url);
            Console.WriteLine("Downloading plugin ID " + arggies[3]);
            WebClient wc = new WebClient();
            wc.DownloadFile(url, to);
            string token = HandoffToken.GetToken();
            string urlRl = "http://localhost:5754/handoff/" + token + "/reload-plugins";
            Get(urlRl);
            MessageBox.Show("Successfully downloaded plugin " + arggies[3] +"!");
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
    }
}

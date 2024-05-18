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
            Console.WriteLine("Finding download sources..");
            string DocuPath = HandoffToken.GetOrDefault();
            string FdPath = DocuPath + "\\freedeck\\plugins\\";
            string url = arggies[4];
            url = Uri.UnescapeDataString(url);
            string to = FdPath + arggies[3] + ".Freedeck";
            url = Uri.UnescapeDataString(url);
            Console.WriteLine("Prompting user to download");
            string io = "";
            if (url.Contains("https://content-dl.freedeck.app/hosted")) io = " This is an official source, and can be trusted.";
            DialogResult dialogResult = MessageBox.Show("Download " + arggies[3] + " from " + url + "?" +io, "Freedeck", MessageBoxButtons.YesNo);
            if(dialogResult == DialogResult.Yes)
            {
                Console.WriteLine("Downloading!");
                WebClient wc = new WebClient();
                wc.DownloadFile(url, to);
                string token = HandoffToken.GetToken();
                string urlRl = "http://localhost:5754/handoff/" + token + "/reload-plugins";
                string urlNo = "http://localhost:5754/handoff/" + token + "/notify/Downloaded " + arggies[3];
                Get(urlRl);
                Get(urlNo);
                MessageBox.Show("Successfully downloaded plugin " + arggies[3] + "!", "Freedeck");
            }
            else
            {
                Console.WriteLine("Cancelled by user.");
            }
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

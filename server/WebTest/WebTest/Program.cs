using System;
using System.Net;
using System.IO;
namespace WebTest
{
	class Program
	{
		static void Main(string[] args)
		{
            string sURL;
            sURL = "https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch=Craig%20Noone&format=json";

            WebRequest wrGETURL;
            wrGETURL = WebRequest.Create(sURL);

            /*WebProxy myProxy = new WebProxy("myproxy", 80);
            myProxy.BypassProxyOnLocal = true;

            wrGETURL.Proxy = WebProxy.GetDefaultProxy();*/

            Stream objStream;
            objStream = wrGETURL.GetResponse().GetResponseStream();

            StreamReader objReader = new StreamReader(objStream);

            string sLine = "";
            int i = 0;

            while (sLine != null)
            {
                i++;
                sLine = objReader.ReadLine();
                if (sLine != null)
                    Console.WriteLine("{0}", sLine);
            }
            Console.ReadLine();
        }
	}
}

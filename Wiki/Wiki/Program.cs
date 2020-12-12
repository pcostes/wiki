using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Data;

namespace C__Folder
{
    public class Links
    {
        public IList<string> links { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("requesting");
            GetRequest("na");
            Console.WriteLine("done");
            Console.ReadKey();
        }
        async static Task<string[]> bfs(string root, string target)
        {
            return null;
        }
        async static void GetRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                //string full url = "https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch=Craig%20Noone&format=json"
                //string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=iwlinks&titles=Craig%20Noone";
                string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=links&titles=Burt,%20County%20Donegal";
                using (HttpResponseMessage response = await client.GetAsync(fullUrl))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        //Console.WriteLine(mycontent.IndexOf("link"));
                        //Console.WriteLine(mycontent.IndexOf("]"));
                        //Console.WriteLine(mycontent.Length);
                        string links = mycontent.Substring(mycontent.IndexOf("\"link"), mycontent.IndexOf("]")-mycontent.IndexOf("link")+2);
                        Console.WriteLine(links);
                        DataTable dt = JsonConvert.DeserializeObject<DataTable>(links);
                        //Console.WriteLine(allLinks.links);
                        Console.WriteLine(mycontent);
                        foreach (DataRow row in dt.Rows)
                        {
                            Console.WriteLine(row["ns"] + " - " + row["title"]);
                        }
                        Console.WriteLine("done (inner)");
                    }
                }
            }
        }
    }
}

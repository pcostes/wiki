using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace C__Folder
{
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
                string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&links=plnamespace&titles=Burt,%20County%20Donegal";
                using (HttpResponseMessage response = await client.GetAsync(fullUrl))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();
                        Console.WriteLine(mycontent);
                        Console.WriteLine("done (ineer)");
                    }
                }
            }
        }
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
namespace C__Folder
{
    public class Link
	{
        public int ns;
        public string title;
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
                        Console.WriteLine(mycontent);
                        string key = "\"links\":";
                        int start_pos = mycontent.IndexOf(key) + key.Length;
                        string links = mycontent.Substring(start_pos, mycontent.IndexOf("]") + 1 - start_pos);
                        List<Link> generics = JsonConvert.DeserializeObject<List<Link>>(links);

                        Console.WriteLine(generics);
                        Console.WriteLine("done (ineer)");
                    }
                }
            }
        }
    }
}

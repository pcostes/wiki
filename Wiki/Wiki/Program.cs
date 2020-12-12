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
            Console.WriteLine("--requesting");
            List<string> children = await GetChildren("Burt, County Donegal");
            Console.WriteLine("--children:");
            foreach (string s in children)
			{
                Console.WriteLine(s);
			}
            Console.WriteLine("--done");
            Console.ReadKey();
        }

        async static Task<string[]> bfs(string root, string target)
        {
            return null;
        }
        async static Task<List<string>> GetChildren(string title)
        {
            List<string> children = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=links&titles=" + Uri.EscapeUriString(title);
                using (HttpResponseMessage response = await client.GetAsync(fullUrl))
                {
                    using (HttpContent content = response.Content)
                    {
                        string mycontent = await content.ReadAsStringAsync();

                        string key = "\"links\":";
                        int start_pos = mycontent.IndexOf(key) + key.Length;
                        string link_str = mycontent.Substring(start_pos, mycontent.IndexOf("]") + 1 - start_pos);
                        
                        List<Link> link_list = JsonConvert.DeserializeObject<List<Link>>(link_str);

                        foreach (Link l in link_list)
						{
                            children.Add(l.title);
						}
                        
                    }
                }
            }

            return children; 
        }
    }
}

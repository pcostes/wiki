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
        private static HttpClient client = new HttpClient();
        private static Dictionary<string, string> prop_dict = new Dictionary<string, string>();
        
        static async Task Main(string[] args)
        {
            prop_dict.Add("linkshere", "lh");
            prop_dict.Add("links", "pl");
            Console.WriteLine("--requesting");
            List<string> children = await ParseProperty("Burt, County Donegal", "links");
            Console.WriteLine("--children:");
            foreach (string s in children)
			{
                Console.WriteLine(s);
			}
            Console.WriteLine("--done");
            children = await ParseProperty("Burt, County Donegal", "linkshere");
            Console.WriteLine("--links here:");
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
        async static Task<string> MakeRequest(string title, string property)
		{
            if (!prop_dict.ContainsKey(property))
                throw new KeyNotFoundException(property);

            string prop_val = prop_dict[property];
            string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=" + property + 
                "&"+ prop_val + "limit=500&" + prop_val + "namespace=0&titles=" + Uri.EscapeUriString(title);
            using (HttpResponseMessage response = await client.GetAsync(fullUrl))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    return mycontent;
                }
            }
        }
        async static Task<List<string>> ParseProperty(string title, string property)
        {
            List<string> children = new List<string>();
            string mycontent = await MakeRequest(title, property);

            string key = String.Format("\"{0}\":", property);
            int start_pos = mycontent.IndexOf(key) + key.Length;
            string link_str = mycontent.Substring(start_pos, mycontent.IndexOf("]") + 1 - start_pos);

            List<Link> link_list = JsonConvert.DeserializeObject<List<Link>>(link_str);

            foreach (Link l in link_list)
            {
                children.Add(l.title);
            }

            return children; 
        }
    }
}

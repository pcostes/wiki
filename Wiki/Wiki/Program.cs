using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
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
        private static Dictionary<string, string> prop_dict = new Dictionary<string, string>();
        private static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            prop_dict.Add("linkshere", "lh");
            prop_dict.Add("links", "pl");
            /*Console.WriteLine("--requesting");
            List<string> children = await ParseProperty("Burt, County Donegal", "links");
            Console.WriteLine("--children:");
            foreach (string s in children)
			{
                Console.WriteLine(s);
			}*/

            
            Console.WriteLine("--bfs--");
            List<string> path = await bfs("Burt, County Donegal", "Crimean War");
            string pathS = "";
            foreach (string s in path)
            {
                pathS += s + "-";
            }
            Console.WriteLine(pathS.Substring(0, pathS.Length-1));
            //List<string> children = await ParseProperty("David & Charles", "links");
            Console.WriteLine("--done--");
            Console.ReadKey();
        }

        async static Task<List<String>> bfs(string root, string goal)
        {
            Dictionary<string, string> visited = new Dictionary<string, string>();
            visited.Add(root, "");
            Queue<string> fringe = new Queue<string>();
            fringe.Enqueue(root);
            string pathFinder = root;
            while (fringe.Count > 0)
            {
                string temp = fringe.Dequeue();

                Console.WriteLine(temp);
                if (temp == goal)
                {
                    Console.WriteLine("broke (found path)");
                    pathFinder = temp;
                    break;
                }
                foreach (string child in await ParseProperty(temp, "links"))
                {
                    if (!visited.ContainsKey(child))
                    {
                        visited.Add(child, temp);
                        fringe.Enqueue(child);
                    }

                }
            }
            List<String> toret = new List<string>();
            while(pathFinder != root)
            {
                toret.Add(pathFinder);
                pathFinder = visited[pathFinder];
            }
            toret.Add(root);
            toret.Reverse();
            return toret ;
        }
        async static Task<string> MakeRequest(string title, string property)
		{
            if (!prop_dict.ContainsKey(property))
                throw new KeyNotFoundException(property);

            string prop_val = prop_dict[property];
            string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=" + property +
                "&" + prop_val + "limit=500&" + prop_val + "namespace=0&titles=" + HttpUtility.UrlEncode(title); // encodes space as + sign. Watch out!
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
            if (!mycontent.Contains(key))
                return children;
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

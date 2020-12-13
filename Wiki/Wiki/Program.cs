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
        static HttpClient client = new HttpClient();
        static async Task Main(string[] args)
        {
            Console.WriteLine("--requesting--");
            List<string> children = await GetChildren("Burt, County Donegal");
            Console.WriteLine("--children:");
            foreach (string s in children)
			{
                Console.WriteLine(s);
			}
            Console.WriteLine("--bfs--");
            List<string> path = await bfs("Burt, County Donegal", "Fahan");
            string pathS = "";
            foreach (string s in path)
            {
                pathS += s + "-";
            }
            Console.WriteLine(pathS.Substring(0, pathS.Length-1));
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
                if (temp == goal)
                {
                    Console.WriteLine("broke (found path)");
                    pathFinder = temp;
                    break;
                }
                foreach (string child in await GetChildren(temp))
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
        async static Task<List<string>> GetChildren(string title)
        {
            List<string> children = new List<string>();

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

            return children; 
        }
    }
}

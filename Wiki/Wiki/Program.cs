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
            List<string> path = await BIDIRECTIONALbfs("Burt, County Donegal", "Carlingford Lough");
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

        async static Task<List<String>> BIDIRECTIONALbfs(string root, string goal)
        {
            Dictionary<string, string> visited1 = new Dictionary<string, string>();
            visited1.Add(root, "");
            Dictionary<string, string> buriedNodes1 = new Dictionary<string, string>(visited1);
            Queue<string> fringe1 = new Queue<string>();
            fringe1.Enqueue(root);
            fringe1.Enqueue("\t0");
            Dictionary<string, string> visited2 = new Dictionary<string, string>();
            visited2.Add(goal, "");
            Dictionary<string, string> buriedNodes2 = new Dictionary<string, string>(visited2);
            Queue<string> fringe2 = new Queue<string>();
            fringe2.Enqueue(goal);
            fringe2.Enqueue("\t0");
            string pathFinder = "";
            string viablePath = "";
            while (fringe1.Count > 0 && fringe2.Count > 0)
            {
                if(fringe1.Peek().Substring(0,1) == "\t" && fringe2.Peek().Substring(0,1) == "\t")
                {
                    string depth1 = fringe1.Dequeue();
                    string depth2 = fringe2.Dequeue();
                    fringe1.Enqueue("\t" + (int.Parse(depth1.Substring(1))+1).ToString());
                    fringe2.Enqueue("\t" + (int.Parse(depth2.Substring(1)) + 1).ToString());
                    buriedNodes1 = new Dictionary<string, string>(visited1);
                    buriedNodes2 = new Dictionary<string, string>(visited2);
                    Console.WriteLine(String.Format("queue1={0} queue2={1}", depth1, depth2));
                    if(viablePath != "")
                    {
                        Console.WriteLine("broke using viable path");
                        pathFinder = viablePath;
                        break;
                    }
                }
                if (fringe1.Peek().Substring(0, 1) != "\t") // do not progress this tree if it is deeper than the other
                {
                    string temp1 = fringe1.Dequeue();
                    if (buriedNodes2.ContainsKey(temp1))
                    {
                        Console.WriteLine("broke (found path)");
                        pathFinder = temp1;
                        break;
                    }
                    if (visited2.ContainsKey(temp1))
                    {
                        viablePath = temp1;
                    }
                    foreach (string child in await ParseProperty(temp1, "links"))
                    {
                        if (!visited1.ContainsKey(child))
                        {
                            visited1.Add(child, temp1);
                            fringe1.Enqueue(child);
                        }
                    }
                }
                if (fringe2.Peek().Substring(0, 1) != "\t") // do not progress this tree if it is deeper than the other
                {
                    string temp2 = fringe2.Dequeue();
                    if (buriedNodes1.ContainsKey(temp2))
                    {
                        Console.WriteLine("broke (found path)");
                        pathFinder = temp2;
                        break;
                    }
                    if (visited1.ContainsKey(temp2))
                    {
                        viablePath = temp2;
                    }
                    foreach (string child in await ParseProperty(temp2, "links"))
                    {
                        if (!visited2.ContainsKey(child))
                        {
                            visited2.Add(child, temp2);

                            fringe2.Enqueue(child);
                        }
                    }
                }
                //Console.WriteLine(temp
            }
            string tempfinder = pathFinder;
            List<String> ret = new List<string>();
            while (tempfinder != "")
            {
                ret.Insert(0, tempfinder);
                tempfinder = visited1[tempfinder];
            }
            tempfinder = visited2[pathFinder];
            while (tempfinder != "")
            {
                ret.Add(tempfinder);
                tempfinder = visited2[tempfinder];
            }
            return ret;
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

                //Console.WriteLine(temp);
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

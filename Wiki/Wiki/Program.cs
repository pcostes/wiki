using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using System.Collections.Concurrent;

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
        private static int num_tasks = 0;
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
            long unixTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            List<string> path = await bfs("Burt, County Donegal", "Carlingford Lough");
            Console.WriteLine(String.Format("finished in {0} seconds", (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - unixTime) / 1000));
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
            ConcurrentDictionary<string, string> visited1 = new ConcurrentDictionary<string, string>();
            visited1.GetOrAdd(root, "");
            ConcurrentDictionary<string, string> buriedNodes1 = new ConcurrentDictionary<string, string>(visited1);
            ConcurrentQueue<string> fringe1 = new ConcurrentQueue<string>();
            fringe1.Enqueue(root);
            fringe1.Enqueue("\t0");
            ConcurrentDictionary<string, string> visited2 = new ConcurrentDictionary<string, string>();
            visited2.GetOrAdd(goal, "");
            ConcurrentDictionary<string, string> buriedNodes2 = new ConcurrentDictionary<string, string>(visited2);
            ConcurrentQueue<string> fringe2 = new ConcurrentQueue<string>();
            fringe2.Enqueue(goal);
            fringe2.Enqueue("\t0");
            string pathFinder = "";
            string viablePath = "";
            while (fringe1.Count > 0 && fringe2.Count > 0)
            {
                if (fringe1.Count == 0 || fringe2.Count == 0)
                    continue;
                string peek1 = "";
                string peek2 = "";
                fringe1.TryPeek(out peek1);
                fringe2.TryPeek(out peek2);
                if (peek1.Substring(0,1) == "\t" && peek2.Substring(0,1) == "\t")
                {
                    string depth1 = "";
                    string depth2 = "";
                    fringe1.TryDequeue(out depth1);
                    fringe2.TryDequeue(out depth2);
                    Console.WriteLine(String.Format("queue1={0} queue2={1}", depth1, depth2));
                    fringe1.Enqueue("\t" + (int.Parse(depth1.Substring(1)) + 1).ToString());
                    fringe2.Enqueue("\t" + (int.Parse(depth2.Substring(1)) + 1).ToString());
                    buriedNodes1 = new ConcurrentDictionary<string, string>(visited1);
                    buriedNodes2 = new ConcurrentDictionary<string, string>(visited2);
                    Console.WriteLine(String.Format("queue1={0} queue2={1}", depth1, depth2));
                    if(viablePath != "")
                    {
                        Console.WriteLine("broke using viable path");
                        pathFinder = viablePath;
                        break;
                    }
                }
                if (peek1.Substring(0, 1) != "\t") // do not progress this tree if it is deeper than the other
                {
                    string temp1 = "";
                    fringe1.TryDequeue(out temp1);
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
                    AppendChildren(temp1, fringe1, visited1, "links");

                }
                if (peek2.Substring(0, 1) != "\t") // do not progress this tree if it is deeper than the other
                {
                    string temp2 = ""; 
                    fringe2.TryDequeue(out temp2);
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
                    AppendChildren(temp2, fringe2, visited2, "linkshere");
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
            ConcurrentDictionary<string, string> visited = new ConcurrentDictionary<string, string>();
            visited.GetOrAdd(root, "");
            ConcurrentQueue<string> fringe = new ConcurrentQueue<string>();
            fringe.Enqueue(root);

            string pathFinder = root;
            while (true)
            {
                if (fringe.Count == 0)
                    continue;
                string temp = "";
                bool deq_res = fringe.TryDequeue(out temp);

                //Console.WriteLine(temp);
                if (temp == goal)
                {
                    Console.WriteLine("broke (found path)");
                    pathFinder = temp;
                    break;
                }

                var res = AppendChildren(temp, fringe, visited, "links");
                //Console.WriteLine("ending");
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

        async static Task AppendChildren(string title, ConcurrentQueue<string> fringe, ConcurrentDictionary<string, string> visited, string prop)
		{
            num_tasks++;
            //Thread.Sleep(2000);
            foreach (string child in await ParseProperty(title, prop))
            {
                if (!visited.ContainsKey(child))
                {
                    visited.GetOrAdd(child, title);
                    fringe.Enqueue(child);
                }
            }
            Console.WriteLine("{0}", num_tasks);
            num_tasks--;
        }
        async static Task<string> MakeRequest(string title, string property)
		{
            if (!prop_dict.ContainsKey(property))
                throw new KeyNotFoundException(property);

            string prop_val = prop_dict[property];
            string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=" + property +
                "&" + prop_val + "limit=80&" + prop_val + "namespace=0&titles=" + HttpUtility.UrlEncode(title); // encodes space as + sign. Watch out!
            using (HttpResponseMessage response = await client.GetAsync(fullUrl))
            {
                using (HttpContent content = response.Content)
                {
                    string mycontent = await content.ReadAsStringAsync();
                    return mycontent;
                }
            }
        }
        async static Task<string> MakeRequest(string title, string property, string contVal)
        {
            if (!prop_dict.ContainsKey(property))
                throw new KeyNotFoundException(property);

            string prop_val = prop_dict[property];
            string fullUrl = "https://en.wikipedia.org/w/api.php?action=query&format=json&prop=" + property +
                "&" + prop_val + "limit=80&" + prop_val + "continue=" + contVal + "&" + prop_val + "namespace=0&titles=" + HttpUtility.UrlEncode(title); // encodes space as + sign. Watch out!
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
            if (mycontent.Contains("\"continue\":"))
            {
                if (!prop_dict.ContainsKey(property))
                    throw new KeyNotFoundException(property);
                string prop_val = prop_dict[property];
                start_pos = mycontent.IndexOf(String.Format("\"{0}continue\":", prop_val)) + 14;
                string continuestring = mycontent.Substring(start_pos, mycontent.IndexOf("\"", start_pos)-start_pos);
                //Console.WriteLine(continuestring);
                children.AddRange(ParseContinue(title, property, continuestring).Result);
            }

            return children; 
        }

        async static Task<List<string>> ParseContinue(string title, string property, string continuestring)
        {
            List<string> children = new List<string>();
            string mycontent = await MakeRequest(title, property, continuestring);

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
            if (mycontent.Contains("\"continue\":"))
            {
                if (!prop_dict.ContainsKey(property))
                    throw new KeyNotFoundException(property);
                string prop_val = prop_dict[property];
                start_pos = mycontent.IndexOf(String.Format("\"{0}continue\":", prop_val)) + 14;
                string contstr = mycontent.Substring(start_pos, mycontent.IndexOf("\"", start_pos) - start_pos);
                //Console.WriteLine(contstr);
                children.AddRange(ParseContinue(title, property, contstr).Result);
            }
                return children;
        }
    }
}

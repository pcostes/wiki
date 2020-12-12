using System;
using System.Net.Http;

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
        async static void GetRequest(string url)
        {
            using(HttpClient client = new HttpClient())
            {
                using(HttpResponseMessage response = await client.GetAsync("https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch=Craig%20Noone&format=json"))
                {
                    using(HttpContent content = response.Content)
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

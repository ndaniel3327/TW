using System;
using System.Net.Http;

namespace TW.Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();


            string[] localHostUrl = new string[] { "http://localhost:5000/api/Spotify", "http://localhost:57975/api/Spotify", "http://localhost:5001/api/Spotify",
                "http://localhost:44325/api/Spotify" };

            foreach(var url in localHostUrl)
            {
               

                try 
                {
                    var response = client.GetAsync(url);
                    Console.WriteLine(response.Result.StatusCode);
                }
                catch(AggregateException)
                {
                    Console.WriteLine("Agregate Exception");
                }

               
            }   
            
        }
    }
}

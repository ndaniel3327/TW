using System;
using System.Net.Http;
using TW.Infrastracture.Services.Youtube;

namespace TW.Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var youtubeService = new YoutubeServerService();

            youtubeService.YoutubeAuthorization();
        }
    }
}

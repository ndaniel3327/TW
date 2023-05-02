using System;
using TW.Application.Services;

namespace Tw.Test
{

    internal class Program
    {
        static void Main(string[] args)
        {
            SpotifyService service = new SpotifyService();
            service.Autorization();
        }
    }
}

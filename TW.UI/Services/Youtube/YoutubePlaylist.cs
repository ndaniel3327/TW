using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TW.UI.Services.Youtube
{
    public class YoutubePlaylist
    {
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        public string Id { get; set; }
    }
}

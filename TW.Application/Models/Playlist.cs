﻿using System.Collections.Generic;

namespace TW.Application.Models
{

    public class Playlist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Track> Tracks { get; set; }
    }
}

﻿using System.Collections.Generic;

namespace TW.Infrastructure.Models
{
    public class Track
    {
        public List<Artist> Artists { get; set; }

        public string Artist => string.Join(" and ",Artists);

        public string Name { get; set; }
    }
}
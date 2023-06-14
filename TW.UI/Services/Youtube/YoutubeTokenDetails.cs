﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TW.UI.Services.Youtube
{
    public class YoutubeTokenDetails

    {
        [JsonPropertyName("access_token")]
        public string YoutubeAccessToken { get; set; }
        [JsonPropertyName("refresh_token")]
        public string YoutubeRefreshToken { get; set; }
        [JsonPropertyName("token_type")]
        public string YoutubeTokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public int YoutubeExpiresInSeconds { get; set; }
        public DateTime YoutubeTokenExpirationDate { get; set; }
    }
}
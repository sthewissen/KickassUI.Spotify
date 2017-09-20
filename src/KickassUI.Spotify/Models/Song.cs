using System;
namespace KickassUI.Spotify.Models
{
    public class Song
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public string AlbumImageUrl { get; set; }
        public int LengthInSeconds { get; set; }
    }
}

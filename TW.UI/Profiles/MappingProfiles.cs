using AutoMapper;
using SpotifyAPI.Web;
using TW.UI.Models.Spotify.Data;

namespace TW.Infrastructure.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SimpleArtist, SpotifyArtist>()
                .ForMember(dst => dst.Name, act => act.MapFrom(src => src.Name));
            CreateMap<FullTrack, SpotifyTrack>()
                .ForMember(dst=>dst.Artists,act=>act.MapFrom(src=>src.Artists))
                .ForMember(dst=>dst.Name,act=>act.MapFrom(src=>src.Name));
            //CreateMap<PlaylistTrack<FullTrack>, Playlist>()
            //    .ForMember(dst => dst.Tracks, act => act.MapFrom(src => src.Track));
        }
    }
}

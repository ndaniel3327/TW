using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;
using SpotifyAPI.Web;
using TW.Infrastructure.Models;

namespace TW.Infrastructure.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SimpleArtist, Artist>()
                .ForMember(dst => dst.Name, act => act.MapFrom(src => src.Name));
            CreateMap<FullTrack, Track>()
                .ForMember(dst=>dst.Artists,act=>act.MapFrom(src=>src.Artists))
                .ForMember(dst=>dst.Name,act=>act.MapFrom(src=>src.Name));
            //CreateMap<PlaylistTrack<FullTrack>, Playlist>()
            //    .ForMember(dst => dst.Tracks, act => act.MapFrom(src => src.Track));
        }
    }
}

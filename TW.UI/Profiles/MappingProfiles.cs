using AutoMapper;
using TW.UI.Models;
using TW.UI.Models.Spotify.Data;
using TW.UI.Models.Spotify.View;

namespace TW.Infrastructure.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SpotifyTrack, PlaylistDisplayTracks>()
                .ForMember(destination => destination.Name, action => action.MapFrom(source => source.TrackInfo.Name))
                .ForMember(destination=>destination.ArtistsNames,action=>action.MapFrom(source=>source.TrackInfo.Artists.Select(c=>c.Name)));
        }
    }
}

using AutoMapper;
using TW.UI.Models;
using TW.UI.Models.Spotify.Data;

namespace TW.Infrastructure.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SpotifyTrack, PlaylistDisplayTrack>()
                .ForMember(destination => destination.Name, action => action.MapFrom(source => source.TrackInfo.Name))
                .ForMember(destination => destination.ArtistsNames, action => action.MapFrom(source => source.TrackInfo.Artists.Select(c => c.Name)))
                .ForMember(destination => destination.PopupPlayerImageUri, action => action.MapFrom(source =>
                   source.TrackInfo.SpotifyAlbum.SpotifyImages[1].Url));
        }
    }
}

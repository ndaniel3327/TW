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
            CreateMap<SpotifyTrack, SpotifyTrackView>()
                .ForMember(destination => destination.Name, action => action.MapFrom(source => source.TrackInfo.Name))
                .ForMember(destination=>destination.ArtistsNames,action=>action.MapFrom(source=>source.TrackInfo.Artists.Select(c=>c.Name)));

            CreateMap<SpotifyTrackView, PlaylistDisplayTracks>();
            CreateMap<SpotifyPlaylistGroup, PlaylistDisplayGroup>();
                //.ConstructUsing(x=> new PlaylistDisplayGroup(x.Id,x.Name,x.Tracks));
                //.ForCtorParam("tracks",
                //options => options.MapFrom(source => source.Tracks))
                //.ForCtorParam("id",
                //options=>options.MapFrom(source=>source.Id))
                //.ForCtorParam("name",
                //options=>options.MapFrom(source => source.Name));

        }
    }
}

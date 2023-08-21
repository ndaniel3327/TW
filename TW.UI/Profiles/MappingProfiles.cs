using AutoMapper;
using TW.UI.Models;
using TW.UI.Models.Spotify.Data;

namespace TW.Infrastructure.Profiles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //var uri = new Uri("https://i.scdn.co/image/ab67616d00001e025c29a88ba5341ca428f0c322");
            var client = new HttpClient();
            //var wa = await client.GetStreamAsync(uri);

            //mainImage.Source = ImageSource.FromStream(async (x) => wa);


            CreateMap<SpotifyTrack, PlaylistDisplayTrack>()
                .ForMember(destination => destination.Name, action => action.MapFrom(source => source.TrackInfo.Name))
                .ForMember(destination => destination.ArtistsNames, action => action.MapFrom(source => source.TrackInfo.Artists.Select(c => c.Name)))
                .ForMember(destination => destination.PopupPlayerImageUri, action => action.MapFrom(source =>
                   source.TrackInfo.SpotifyAlbum.SpotifyImages[1].Url));

        }

    }
}

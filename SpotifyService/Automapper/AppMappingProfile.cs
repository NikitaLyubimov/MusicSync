using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using SpotifyLib.DTO.Tracks;
using SpotifyService.DTOs.Response;

namespace SpotifyService.Automapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserItemInfo, TrackDtoResponse>()
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.Name))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Track.Artists[0].Name));
            CreateMap<GetTracksResponse, TracksForQueueResponse>();

            CreateMap<UserItemInfo, TrackMetadataResponse>()
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.Name))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Track.Artists[0].Name))
                .ForMember(dest => dest.TrackImages, opt => opt.MapFrom(src => src.Track.Album.Images))
                .ForMember(dest => dest.AlbumName, opt => opt.MapFrom(src => src.Track.Album.Name));
            CreateMap<GetTracksResponse, TracksMetadataResponse>();
        }
    }
}

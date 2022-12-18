using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using SpotifyLib.DTO.Playlists;
using SpotifyLib.DTO.Tracks;
using SpotifyService.DTOs.Response;

using CoreLib.Playlists;
using CoreLib.TracksDTOs;

namespace SpotifyService.Automapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<UserItemInfo, TrackDtoResponse>()
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.Name))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Track.Artists[0].Name))
                .ForSourceMember(src => src.AddedAt, opt => opt.DoNotValidate());
            CreateMap<GetTracksResponse, TracksForQueueDto>()
                .ForMember(dest => dest.Tracks, opt => opt.MapFrom(src => src.Items))
                .ForSourceMember(src => src.Total, opt => opt.DoNotValidate());

            CreateMap<UserItemInfo, TrackMetadataResponse>()
                .ForMember(dest => dest.TrackName, opt => opt.MapFrom(src => src.Track.Name))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Track.Artists[0].Name))
                .ForMember(dest => dest.TrackImages, opt => opt.MapFrom(src => src.Track.Album.Images))
                .ForMember(dest => dest.AlbumName, opt => opt.MapFrom(src => src.Track.Album.Name));
            CreateMap<GetTracksResponse, TracksMetadataResponse>();

            CreateMap<PlaylistItem, PlaylistForQueue>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Tracks, opt => opt.Ignore())
                .ForSourceMember(src => src.Href, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Images, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Tracks, opt => opt.DoNotValidate());

            CreateMap<PlaylistsListResponse, PlaylistsForQueueDto>()
                .ForMember(dest => dest.Playlists, opt => opt.MapFrom(src => src.Items))
                .ForSourceMember(src => src.Href, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Total, opt => opt.DoNotValidate());

                
        }
    }
}

using AutoMapper;
using OutlouTest.Models;
using System.ServiceModel.Syndication;

namespace OutlouTest.MapperProfiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<SyndicationItem, FeedItem>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Text))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary.Text))
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Links[0].Uri))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PublishDate, opt => opt.MapFrom(src => src.PublishDate.DateTime));
        }
    }
}
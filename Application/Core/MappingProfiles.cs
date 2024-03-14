  using Application.Activity;
using Application.Comments;
using Application.Dtos;
using Application.Notifications;
using AutoMapper;
using Domain;
using Domain.OrderAggregate;
using System.Linq;

namespace Application.Core
{
    //public class MappingProfiles : Profile
    //{
    //    public MappingProfiles()
    //    {
            
    //    }
    //    private readonly IMapper mapper;
    //    public MappingProfiles(IMapper mapper)
    //    {
    //        string currentUsername = null;
    //        CreateMap<Activities, Activities>();
    //        CreateMap<Activities, ActivityDto>().ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees
    //               .FirstOrDefault(x => x.IsHost).AppUser.UserName));

    //        CreateMap<ActivityAttendee, AttendeeDto>()
    //            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
    //            .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
    //            .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
    //            .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Phots.FirstOrDefault(x => x.IsMain).Url))
    //            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
    //            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
    //            .ForMember(d => d.Following,
    //                o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername))); ; ;

    //        CreateMap<AppUser, Profiles.Profile>()
    //        .ForMember(d => d.Image, o => o.MapFrom(s => s.Phots.FirstOrDefault(x => x.IsMain).Url))
    //        .ForMember(d => d.Photos, o => o.MapFrom(s => s.Phots))
    //        .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
    //        .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
    //        .ForMember(d => d.Following,
    //            o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername))); ;


    //        CreateMap<Comment, CommentDto>()
    //          .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
    //          .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
    //          .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Phots.FirstOrDefault(x => x.IsMain).Url));


    //        CreateMap<ActivityAttendee, Profiles.UserActivityDto>()
    //          .ForMember(d => d.Id, o => o.MapFrom(s => s.Activity.Id))
    //          .ForMember(d => d.Date, o => o.MapFrom(s => s.Activity.Date))
    //          .ForMember(d => d.Title, o => o.MapFrom(s => s.Activity.Title))
    //          .ForMember(d => d.Category, o => o.MapFrom(s => s.Activity.Category))
    //          .ForMember(d => d.HostUsername, o => o.MapFrom(s =>
    //              s.Activity.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));

    //        CreateMap<AddressDto, OrderAddress>().ReverseMap();
    //        CreateMap<Domain.OrderAggregate.Order, OrderDto>()
    //          .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
    //          .ForMember(d => d.ShippingAddress.FirstName, o => o.MapFrom(s => s.ShipToAddress.FirstName))
    //          .ForMember(d => d.ShippingAddress.LastName, o => o.MapFrom(s => s.ShipToAddress.LastName))
    //          .ForMember(d => d.ShippingAddress.City, o => o.MapFrom(s => s.ShipToAddress.City))
    //          .ForMember(d => d.ShippingAddress.ZipCode, o => o.MapFrom(s => s.ShipToAddress.ZipCode))
    //          .ForMember(d => d.ShippingAddress.Street, o => o.MapFrom(s => s.ShipToAddress.Street))
    //          .ForMember(d => d.ShippingAddress.State, o => o.MapFrom(s => s.ShipToAddress.State));

    //        CreateMap<Notification, NotificationDto>();
    //        CreateMap<Address, AddressDto>().ReverseMap();
    //        CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
    //        CreateMap<BasketItemDto, BasketItem>();


    //        this.mapper = mapper;
    //    }


    //}
}
using API.DTOs;
using Application.Activity;
using Application.Comments;
using Application.Notifications;
using Application.Profiles;
using AutoMapper;
using Domain;
using Domain.OrderAggregate;
using static StackExchange.Redis.Role;

namespace API.Helpers
{
    public class MappingProfiles : AutoMapper.Profile
    {
      
        public MappingProfiles()
        {
            
            //CreateMap<Product, ProductToReturnDto>()
            //    .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
            //    .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
            //    .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());


            //CreateMap<CommandProductDto, Product>()
            //.ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => new ProductBrand { Name = src.ProductBrand }))
            //.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => new ProductType { Name = src.ProductType }))
            //.ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>();
            string currentUsername = null;
            CreateMap<Activities, Activities>();
            CreateMap<Activities, ActivityDto>().ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Attendees
                   .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Phots.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
                .ForMember(d => d.Following,
                    o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername))); ; ;

            CreateMap<AppUser, Application.Profiles.Profile>()
            .ForMember(d => d.Image, o => o.MapFrom(s => s.Phots.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.Photos, o => o.MapFrom(s => s.Phots))
            .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
            .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
            .ForMember(d => d.Following,
                o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername))); ;


            CreateMap<Comment, CommentDto>()
              .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
              .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
              .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Phots.FirstOrDefault(x => x.IsMain).Url));


            CreateMap<ActivityAttendee, Application.Profiles.UserActivityDto>()
              .ForMember(d => d.Id, o => o.MapFrom(s => s.Activity.Id))
              .ForMember(d => d.Date, o => o.MapFrom(s => s.Activity.Date))
              .ForMember(d => d.Title, o => o.MapFrom(s => s.Activity.Title))
              .ForMember(d => d.Category, o => o.MapFrom(s => s.Activity.Category))
              .ForMember(d => d.HostUsername, o => o.MapFrom(s =>
                  s.Activity.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));
            
            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<Domain.OrderAggregate.Order, Application.Dtos.OrderDto>()
              .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
              .ForPath(d => d.ShippingAddress.FirstName, o => o.MapFrom(s => s.ShipToAddress.FirstName))
              .ForPath(d => d.ShippingAddress.LastName, o => o.MapFrom(s => s.ShipToAddress.LastName))
              .ForPath(d => d.ShippingAddress.City, o => o.MapFrom(s => s.ShipToAddress.City))
              .ForPath(d => d.ShippingAddress.ZipCode, o => o.MapFrom(s => s.ShipToAddress.ZipCode))
              .ForPath(d => d.ShippingAddress.Street, o => o.MapFrom(s => s.ShipToAddress.Street))
              .ForPath(d => d.ShippingAddress.State, o => o.MapFrom(s => s.ShipToAddress.State))
              .ForMember(d => d.DeliveryMethodId, o => o.MapFrom(s => s.DeliveryMethod.Id))
              .ReverseMap();

            CreateMap<Notification, NotificationDto>();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<ApiMovie, Movie>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Overview))
                .ForMember(dest => dest.DateOfRelease, opt => opt.MapFrom(src => src.ReleaseDate))
                .ForMember(dest => dest.CoverPhoto, opt => opt.MapFrom(src => src.PosterPath))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Director, opt => opt.MapFrom(src => src.Director))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.VoteAverage))
                .ForMember(dest => dest.Genres, 
                    opt => opt.MapFrom(src => src.Genres.Select(genre => genre.Name).ToList()))
                .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.Actors != null ? src.Actors.Select(apiAct => new Actor()
                {
                    Id = apiAct.Id,
                    Name = apiAct.Name
                }).ToList() : new List<Actor>()));







        }
    }
}

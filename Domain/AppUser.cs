using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public string Bio { get; set; }

        public string? PartnerId { get; set; }
        public AppUser Partner { get; set; }
        public ICollection<ActivityAttendee> Activities  { get; set; }     
        public ICollection<Photo> Phots  { get; set; }
        public ICollection<UserFollowing> Followings { get; set; }
        public ICollection<UserFollowing> Followers { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<UserMovieLike> Likes { get; set; } = new List<UserMovieLike>();
        public ICollection<UserWatchedMovie> WatchedMovies { get; set; } = new List<UserWatchedMovie>();
        public ICollection<DateNight> DateNights { get; set; } = new List<DateNight>();


    }
}
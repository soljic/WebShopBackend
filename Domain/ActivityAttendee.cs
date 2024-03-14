using System;

namespace Domain
{
    public class ActivityAttendee
    {
        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public Guid ActivityId { get; set; }

        public Activities Activity { get; set; }
        
        public bool IsHost { get; set; }
    }
}
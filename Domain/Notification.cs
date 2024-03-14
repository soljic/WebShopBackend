using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Notification
    {
        public int Id { get; set; }
        public string Body { get; set; }

        [ForeignKey("AuthorId")]
        public AppUser Author { get; set; }

        [ForeignKey("ReceieverId")]
        public AppUser Receiever { get; set; }

        public int DateNightId { get; set; }
        public DateNight DateNight { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}

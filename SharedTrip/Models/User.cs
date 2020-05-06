using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SharedTrip.Models
{
    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserTrips = new List<UserTrip>();
        }

        public string Id { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }


        public string Password { get; set; }

        public virtual ICollection<UserTrip> UserTrips { get; set; }

    }
}

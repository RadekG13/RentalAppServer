using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp3.Models
{
    public class AppUser : IdentityUser
    {
        

        public string NameAndSurname { get; set; }

        public string AccountNumber { get; set; }

        public ICollection<Apartment> Apartments { get; set; }

    }
}

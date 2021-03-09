using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp3.Models
{
    public class Apartment
    {
       
        public string ApartmentId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string AppUserID { get; set; }
        public virtual AppUser AppUser { get; set; }


        public byte[] Photo { get; set; }

        


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RentalApp3.Models
{
    public class Room
    {

        public string RoomId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Deposit { get; set; }

        public int RentFee { get; set; }

        public bool Status { get; set; }

        public string ApartmentId { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual Apartment Apartment { get; set; }


        public byte[] Photo { get; set; }



    }
}

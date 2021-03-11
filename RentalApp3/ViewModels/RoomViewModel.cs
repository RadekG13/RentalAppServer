using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp3.ViewModels
{
    public class RoomViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int Deposit { get; set; }

        [Required]
        public int RentFee { get; set; }

        [Required]
        public string ApartmentTitle { get; set; }


        [Required]
        public IFormFile Photo { get; set; }



    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp3.ViewModels
{
    public class ApartmentViewModel
    {
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }

    }
}

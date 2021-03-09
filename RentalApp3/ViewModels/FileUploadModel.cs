using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp3.ViewModels
{
    public class FileUploadModel
    {
        public int Id { get; set; }

        public IFormFile file {get; set;}

        public string Apartments { get; set; }

    }
}

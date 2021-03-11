using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RentalApp3.Models;
using RentalApp3.Services;
using RentalApp3.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RentalApp3.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        IAparmentService _service = null;
        public ApartmentsController(IAparmentService service)
        {
            _service = service;
        }

        [HttpPost("saveapartment")]
        public async Task<IActionResult> SaveFile([FromForm] FileUploadModel fileObj)
        {
            Apartment apartment = JsonConvert.DeserializeObject<Apartment>(fileObj.Apartments);

            apartment.AppUserID = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            apartment.ApartmentId = Guid.NewGuid().ToString();

            if (fileObj.file.Length > 0)
            {


                using (var ms = new MemoryStream())
                {
                    if (ms.Length < 2097152) // Upload the file if less than 2 MB
                    {
                        fileObj.file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        apartment.Photo = fileBytes;


                        var result = await _service.AddApartment(apartment);
                        if (result.IsSuccess)
                        {
                            return Ok(result);
                        }
                        return BadRequest(result);
                    }
                    else
                    {
                        return BadRequest("The Photo size is too large");
                    }
                }
            }
            return BadRequest("something else doesn't work"); //sprawdzic co


        }

        [HttpGet("getapartment")]
        public string GetSavedApartmentsByUserId() ///tutaj sprawdzic
        {

            string ID = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var apartments = _service.GetSavedApartments(ID);

            foreach (Apartment apartment in apartments)
            {
                apartment.Photo = this.GetImage(Convert.ToBase64String(apartment.Photo));
            }


            return JsonConvert.SerializeObject(apartments);
        }

        public byte[] GetImage(string sBase64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(sBase64String))
            {
                bytes = Convert.FromBase64String(sBase64String);
            }
            return bytes;
        }


        [HttpPost("room")]
        public async Task<IActionResult> SaveRoom([FromForm] RoomUploadModel fileObj)
        {
            Room room = JsonConvert.DeserializeObject<Room>(fileObj.Rooms);

            room.RoomId = Guid.NewGuid().ToString();

            string UserID = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            string apartmentId = _service.GetApartmentId(UserID, fileObj.ApartmentTitle);

            room.ApartmentId = apartmentId;


            if (fileObj.file.Length > 0)
            {


                using (var ms = new MemoryStream())
                {
                    if (ms.Length < 2097152) // Upload the file if less than 2 MB
                    {
                        fileObj.file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        room.Photo = fileBytes;


                        var result = await _service.AddRoom(room);
                        if (result.IsSuccess)
                        {
                            return Ok(result);
                        }
                        return BadRequest(result);
                    }
                    else
                    {
                        return BadRequest("The Photo size is too large");
                    }
                }
            }
            return BadRequest("something else doesn't work"); //sprawdzic co



        }
    }
}

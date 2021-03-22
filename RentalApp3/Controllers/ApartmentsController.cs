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

        [HttpPost]
        public async Task<IActionResult> CreateApartment([FromForm] FileUploadModel fileObj)
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

        [HttpGet]
        public ActionResult GetApartments() ///tutaj sprawdzic
        {

            string ID = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var apartments = _service.GetSavedApartments(ID);

            //foreach (Apartment apartment in apartments)
            //{
            //    apartment.Photo = this.GetImage(Convert.ToBase64String(apartment.Photo)); //czy to ma sens? XDDDDDDDDD
            //}
            if (apartments == null)
            {
                return NotFound();
            }


            return Ok(JsonConvert.SerializeObject(apartments));
           // return Ok(apartments); //tak nie działa
        }


        //czyli to też bez sensu
        public byte[] GetImage(string sBase64String)
        {
            byte[] bytes = null;
            if (!string.IsNullOrEmpty(sBase64String))
            {
                bytes = Convert.FromBase64String(sBase64String);
            }
            return bytes;
        }



        [HttpGet("{id}")]
        public ActionResult GetApartmentById(string id)
        {
           string UserId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


           var apartment = _service.GetApartment(UserId, id);

            if (apartment == null)
            {
               return NotFound();
            }

           return Ok(JsonConvert.SerializeObject(apartment));

        }

       [HttpPut]
        public IActionResult EditApartment([FromForm] FileUploadModel fileObj)
        {
            string UserId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            try
            {
                if (fileObj==null || !ModelState.IsValid)
                {
                    return BadRequest("Wrong or null object");
                }

                var existingApartment = _service.GetApartment(UserId, fileObj.Id);

                if(existingApartment==null)
                {
                    return NotFound("Apartment do not exist");
                }

                Apartment apartment = JsonConvert.DeserializeObject<Apartment>(fileObj.Apartments);
                apartment.AppUserID = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                apartment.ApartmentId = fileObj.Id;

                if (fileObj.file.Length > 0)
                {

                    using (var ms = new MemoryStream())
                    {
                        if (ms.Length < 2097152) // Upload the file if less than 2 MB
                        {
                            fileObj.file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            apartment.Photo = fileBytes;


                            var result =  _service.UpdateApartment(apartment);
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
                return BadRequest("Miss Photo");


            }
            catch(Exception)
            { return BadRequest("Could not update"); }

           
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteApartment(string id)
        {
            string UserId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            try
            {
                var apartment = _service.GetApartment(UserId, id);
                if (apartment == null)
                {
                    return NotFound();
                }
                var result = _service.DeleteApartment(id);
                if (result.IsSuccess)
                {
                    return Ok(result);
                }
                return BadRequest(result);

            }
            catch (Exception)
            {
                return BadRequest("Could not delete");
            }
         


        }
        
        //------------------------------------------------------------------------------------------------------------------

        [HttpPost("rooms")] 
        public async Task<IActionResult> CreateRoom([FromForm] RoomUploadModel fileObj)
        {
            Room room = JsonConvert.DeserializeObject<Room>(fileObj.Rooms);

            room.RoomId = Guid.NewGuid().ToString();

            string userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            


            Apartment apartment = _service.GetApartmentById(fileObj.ApartmentId);

            if (apartment.AppUserID == userId)
            {

                room.ApartmentId = fileObj.ApartmentId; //tutaj można chyba do room dać w androidzie, ale to trzeba zmienić roomupload model
                room.Status = false;

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
            else
            {
                return BadRequest("Wrong user!");
            }


        }

        [HttpGet("rooms")] 
        public IActionResult GetRooms(string apartmentId)
        {
            string UserID = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var apartment = _service.GetApartmentById(apartmentId);
            if (apartment.AppUserID == UserID)
            {
                var rooms = _service.GetSavedRooms(apartmentId); 
                return Ok(JsonConvert.SerializeObject(rooms));
            }
            return BadRequest("Wrong user!");


        }


        [HttpPut("rooms/{id}")] 
        public IActionResult EditRoom(string id, [FromForm] RoomUploadModel fileObj)
        {

            string UserId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


            try
            {
                if (fileObj == null || !ModelState.IsValid)
                {
                    return BadRequest("Wrong or null object");
                }

                var existingRoom = _service.GetSavedRoom(fileObj.ApartmentId, id);

                if (existingRoom == null)
                {
                    return NotFound("Room do not exist");
                }

                Room room = JsonConvert.DeserializeObject<Room>(fileObj.Rooms);
            
                room.RoomId = id;
                room.ApartmentId = fileObj.ApartmentId;


                if (fileObj.file.Length > 0)
                {

                    using (var ms = new MemoryStream())
                    {
                        if (ms.Length < 2097152) // Upload the file if less than 2 MB
                        {
                            fileObj.file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            room.Photo = fileBytes;


                            var result = _service.UpdateRoom(room);
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
                return BadRequest("Miss Photo");


            }
            catch (Exception)
            { return BadRequest("Could not update"); }















        }

        [HttpDelete("rooms/{id}")]
        public IActionResult DeleteRoom(string id, string apartmentId)
        {
            string userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            Apartment apartment = _service.GetApartmentById(apartmentId); //dodać sprawdzanie czy apartment nie jest null

            if (apartment.AppUserID == userId)
            {


                try
                {
                    var room = _service.GetSavedRoom(apartmentId, id);


                    if (room == null)
                    {
                        return NotFound();
                    }
                    var result = _service.DeleteRoom(id);
                    if (result.IsSuccess)
                    {
                        return Ok(result);
                    }
                    return BadRequest(result);

                }
                catch (Exception)
                {
                    return BadRequest("Could not delete");
                }
            }
            else
            {
                return BadRequest("Wrong user"); 
            }
        }


    }
}

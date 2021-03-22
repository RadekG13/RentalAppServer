using RentalApp3.Models;
using RentalApp3.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RentalApp3.Services
{
    public interface IAparmentService
    {
        Task<Response2> AddApartment(Apartment model);
        IEnumerable<Apartment> GetSavedApartments(string ID);
        Apartment GetApartment(string userId, string id);
        Response2 UpdateApartment(Apartment apartment);
        Response2 DeleteApartment(string id);
        Apartment GetApartmentById(string ID);
        //-------------------------------------------------------
        Task<Response2> AddRoom(Room model);
        IEnumerable<Room> GetSavedRooms( string ID);
        Room GetSavedRoom(string apartmentId, string roomId);
        Response2 DeleteRoom(string id);
        Response2 UpdateRoom(Room room);
    }


    
    public class ApartmentService : IAparmentService
    {
        private readonly AppDbContext _context;

        public ApartmentService(AppDbContext context)
        {
            _context = context;
        }



        public async Task<Response2> AddApartment(Apartment model)
        {
            var result = await _context.apartments.AddAsync(model);
            _context.SaveChanges(); //czy tutaj nie powinno być async?

            if (_context.apartments.Any(o => o.ApartmentId==model.ApartmentId))
            {
                return new Response2
                {
                    Message = "Apartment created!",
                    IsSuccess = true,
                };
            }
            return new Response2
            {
                Message = "Apartment did not create:",
                IsSuccess = false,

            };


        }

        

        public IEnumerable<Apartment> GetSavedApartments(string ID)
        {
            return _context.apartments.Where(t => t.AppUserID == ID).AsEnumerable();
        }

        
       
        public Apartment GetApartment(string userId, string id)
        {
            return _context.apartments.Where(t => t.AppUserID == userId)
                .FirstOrDefault(t => t.ApartmentId == id);

        }


        //to bez sensu
        public Apartment GetApartmentById(string ID)
        {




            return _context.apartments.FirstOrDefault(t => t.ApartmentId == ID);

          

        }


       

        public  Response2 UpdateApartment(Apartment apartment)
        {

            var entity = _context.apartments.Where(o=>o.AppUserID==apartment.AppUserID)
                .FirstOrDefault(item => item.ApartmentId == apartment.ApartmentId); //dodać sprawdzanie użytkownika

            if(entity != null)
            {
                entity.Title = apartment.Title;
                entity.Description = apartment.Description;
                entity.Photo = apartment.Photo;

                _context.SaveChanges();


                return new Response2
                {
                    Message = "Apartment updated!", //zmienić to później
                    IsSuccess = true,
                };

            }
            return new Response2
            {
                Message = "Apartment did not update!", //zmienić to później
                IsSuccess = false,
            };

            //  var result =  _context.apartments.Update(apartment); //tutej wywalo błąd


        }

        public Response2 DeleteApartment(string id)
        {

            //usunąć mieszkanie i pokoje


            var apartment = _context.apartments.FirstOrDefault(t => t.ApartmentId == id);


             _context.apartments.Remove(apartment);


            _context.SaveChanges();

            if (_context.apartments.Any(o => o.ApartmentId == id))
            {
                return new Response2
                {
                    Message = "Apartment did not delete:",
                    IsSuccess = false,
                };
            }
            return new Response2
            {
                Message = "Apartment deleted!",
                IsSuccess = true,
               

            };

        }

        //----------------------------------------------------------------------------------------------------------------
        public IEnumerable<Room> GetSavedRooms( string ID)
        {
            return _context.rooms.Where(t => t.ApartmentId == ID).AsEnumerable();
        }


        public async Task<Response2> AddRoom(Room model)
        {
            var result = await _context.rooms.AddAsync(model);
            _context.SaveChanges();

            if (_context.rooms.Any(o => o.RoomId == model.RoomId))
            {
                return new Response2
                {
                    Message = "Room created!",
                    IsSuccess = true,
                };
            }
            return new Response2
            {
                Message = "Room did not create:",
                IsSuccess = false,

            };
        }

        public Room GetSavedRoom(string apartmentId, string roomId)
        {
             

            return _context.rooms.Where(t => t.ApartmentId == apartmentId)
                .FirstOrDefault(t => t.RoomId == roomId);
        }

        public Response2 DeleteRoom(string id)
        {
             

            var rooms = _context.rooms.FirstOrDefault(t => t.RoomId == id);
           _context.rooms.Remove(rooms);


            _context.SaveChanges();

            if (_context.rooms.Any(o => o.RoomId == id))
            {
                return new Response2
                {
                    Message = "Room did not delete:",
                    IsSuccess = false,
                };
            }
            return new Response2
            {
                Message = "Room deleted!",
                IsSuccess = true,


            };


        }

        public Response2 UpdateRoom(Room room)
        {
           

            var entity = _context.rooms.Where(t => t.ApartmentId == room.ApartmentId)
                .FirstOrDefault(testc => testc.RoomId == room.RoomId);


            if (entity != null)
            {
                entity.Title = room.Title;
                entity.Description = room.Description;
                entity.Photo = room.Photo;
                entity.RentFee = room.RentFee;
                entity.Deposit = room.Deposit;

                _context.SaveChanges();


                return new Response2
                {
                    Message = "Apartment updated!", //zmienić to później
                    IsSuccess = true,
                };

            }
            return new Response2
            {
                Message = "Apartment did not update!", //zmienić to później
                IsSuccess = false,
            };

            //  var result =  _context.apartments.Update(apartment); //tutej wywalo błąd
        }
    }


}

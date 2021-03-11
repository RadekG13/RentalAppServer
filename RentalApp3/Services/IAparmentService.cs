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

        Task<Response2> AddRoom(Room model);
        string GetApartmentId(string ID, string title);

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

        public async Task<Response2> AddRoom(Room model)
        {
            var result = await _context.rooms.AddAsync(model);
            _context.SaveChanges(); 

            if (_context.rooms.Any(o=>o.RoomId==model.RoomId))
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

        public IEnumerable<Apartment> GetSavedApartments(string ID)
        {
            return _context.apartments.Where(t => t.AppUserID == ID).AsEnumerable();
        }

        public string GetApartmentId(string ID, string title)
        {
            IEnumerable<Apartment> apartments = GetSavedApartments(ID);

            foreach (Apartment apartment in apartments)
            {
                if (apartment.Title == title)
                {
                    return apartment.ApartmentId;
                }
            }

            return null; ///???????????

             }

    }


}

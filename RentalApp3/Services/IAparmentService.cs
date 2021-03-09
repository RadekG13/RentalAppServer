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
    }


}

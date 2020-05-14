using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using MotorGliding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MotorGliding.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly MotorGlidingContext _context;

        public VehicleService(MotorGlidingContext context)
        {
            _context = context;
        }

        public async Task<IList<Vehicle>> ListAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetMainAsync()
        {
            return await _context.Vehicles.Include(f => f.Features).Include(i => i.Images).FirstAsync();
            //return await _context.Vehicles.Include(f => f.Features.Any(id => id.SourceId.Equals(f.Id))).Include(i => i.Images).FirstAsync();
        }

        public async Task<bool> AddAsync(Vehicle vehicle) 
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            vehicle.Features1.Name = "Przyśpieszenie";
            vehicle.Features2.Name = "Moc";
            vehicle.Features3.Name = "Zwrotność";
            vehicle.Features4.Name = "Nośność";
            vehicle.Features = new List<Features>() { vehicle.Features1, vehicle.Features2, vehicle.Features3, vehicle.Features4 };
            foreach(var item in vehicle.Features)
            {
                item.SourceId = vehicle.Id;
                await _context.Features.AddAsync(item);
            }
            vehicle.Image = new Image() { Category = "Vehicle", Default = true, SourceId = vehicle.Id, Name = $"~/gallery/MainLogo_{vehicle.Id}" };
            await _context.Images.AddAsync(vehicle.Image);
            return await _context.SaveChangesAsync() > 0;
        }    
    }
}

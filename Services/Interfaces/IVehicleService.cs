using Microsoft.AspNetCore.Mvc;
using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IList<Vehicle>> ListAsync();
        Task<IList<Vehicle>> GetMainAsync();
        Task<int> AddAsync(Vehicle vehicle);
        Task<int> UpdateAsync(Vehicle vehicle);
        Task<bool> RemoveAsync(int id);
        Task<Vehicle> GetAsync(int id);

    }
}

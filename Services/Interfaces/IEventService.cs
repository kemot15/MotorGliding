using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.Interfaces
{
    public interface IEventService
    {
        Task<IList<Event>> ListAsync();
        //Task<IList<Event>> GetEventsVisibleAsync();
        Task<bool> CreateAsync(Event model);
        Task<bool> RemoveAsync(int id);
        Task<bool> UpdateAsync(Event model);
        Task<Event> GetAsync(int id);
    }
}

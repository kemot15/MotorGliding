using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using MotorGliding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services
{
    public class EventService : IEventService
    {
        private readonly MotorGlidingContext _context;

        public EventService(MotorGlidingContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Event model)
        {
            await _context.Events.AddAsync(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Event model)
        {
            _context.Events.Update(model);        
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Event> GetAsync(int id)
        {
            return await _context.Events.SingleOrDefaultAsync(e => e.Id == id);
        }

        //public async Task<IList<Event>> GetEventsVisibleAsync()
        //{
        //    return await _context.Events.Where(e => e.Visible == true).ToListAsync();
        //}

        public async Task<IList<Event>> ListAsync()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = await GetAsync(id);
            if (item == null)
                return false;
            _context.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }       
    }
}

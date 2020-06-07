using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.Editing;
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

        public async Task<int> CreateAsync(Event model)
        {
            await _context.Events.AddAsync(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task<int> UpdateAsync(Event model)
        {
            _context.Events.Update(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }
        /// <summary>
        /// Pobiera Event z lista obrazow
        /// </summary>
        /// <param name="id">Id Eventu do pobrania</param>
        /// <returns>Zwraca pobrany Event</returns>
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
            //var list = await _context.Events.ToListAsync();
            //foreach(var item in list)
            //{
            //    item.Image = await get
            //}

            return await _context.Events.Include(g => g.Gallery).ToListAsync();
        }

        /// <summary>
        /// Usuwa wybrany Event
        /// </summary>
        /// <param name="id">Id Eventu do usunięcia</param>
        /// <returns>Zwraca true przy powodzeniu usuniecia</returns>
        public async Task<bool> RemoveAsync(Event item)
        {           
            _context.Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }       
    }
}

using Microsoft.EntityFrameworkCore;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly MotorGlidingContext _context;

        public CalendarService(MotorGlidingContext context)
        {
            _context = context;
        }

        public async Task<bool> ReserveDayAsync(DateTime date)
        {
            var day = new Calendar() { Present = date };
            _context.Calendar.Add(day);
            return await _context.SaveChangesAsync() > 0;
        }      

        public async Task<bool> CancelDayAsync(int id)
        {
            var day = await _context.Calendar.SingleAsync(d => d.Id == id);
            _context.Calendar.Remove(day);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IList<Calendar>> GetReservationAsync(DateTime date)
        {
            return await _context.Calendar.Where(d => d.Present.Month == date.Month).ToListAsync();
        }

        public async Task<bool> ClearReservedAsync(DateTime date)
        {
            IList<Calendar> list = await GetReservationAsync(date);
            foreach(var item in list)
            {
                if (item.Present >= DateTime.Now)
                    _context.Remove(item);
                
            }
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

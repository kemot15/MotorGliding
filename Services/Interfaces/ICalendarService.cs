using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services
{
    public interface ICalendarService
    {
        Task<bool> ReserveDayAsync(DateTime date);
        Task<bool> CancelDayAsync (int id);
        Task<IList<Calendar>> GetReservationAsync(DateTime date);
        Task<bool> ClearReservedAsync(DateTime date);

    }
}

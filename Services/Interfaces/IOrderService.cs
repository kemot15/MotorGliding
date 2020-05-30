using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateAsync(Order order);
        Task<Order> GetAsync(int id);
        Task<bool> UpdateAsync(Order order);
        Task<bool> RemoveAsync(int id);
        Task<OrderDetails> GetOrderDetailsAsync(int id);
        Task<bool> UpdateOrderDetailsAsync(List<OrderDetails> detail);
        Task<int> CreateUserAsync(EditUserViewModel user);
        Task<int> UpdateUserAsync(User user);

        Task<bool> UpdateOrderUserId(int orderId, int userId);
        Task<bool> OrderAccept(int id);
        /// <summary>
        /// Lista zamówien ze szczegółami i użykownikami do wyswietlenia na tablicy głownej
        /// </summary>
        /// <returns>Lista zamowien</returns>
        Task<IList<Order>> GetSummaryOrders();

        /// <summary>
        /// Pobiera zamówienie z szczegółami, użytkownikie, adresem do podglądu
        /// </summary>
        /// <param name="id">ID zamówienia</param>
        /// <returns>Order</returns>
        Task<Order> GetPreviewAsync(int id);
        /// <summary>
        /// Sortuje zamówienia po danym wydarzeniu
        /// </summary>
        /// <param name="id">Id Event</param>
        /// <returns>Zwraca liste zamowien dla danego wydarzenia</returns>
        Task<IList<Order>> FilterOrderContainingEvent(int id);
        /// <summary>
        /// Zamowienia przypisane do uzytkownika
        /// </summary>
        /// <param name="id">Id uzytkownika</param>
        /// <returns>Lista zamowien uzytkownika</returns>
        Task<IList<Order>> FilterOrderByUser(int id);
    }
}

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
    }
}

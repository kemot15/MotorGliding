using MotorGliding.Models.Db;
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
    }
}

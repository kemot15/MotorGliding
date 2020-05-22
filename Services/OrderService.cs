using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using MotorGliding.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services
{
    public class OrderService : IOrderService
    {
        private readonly MotorGlidingContext _context;

        public OrderService(MotorGlidingContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Order> GetAsync(int id)
        {
            return await _context.Orders.Include(d => d.OrderDetails).SingleAsync(o => o.Id == id);
        }

        public async Task<bool> UpdateAsync (Order order)
        {
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() >0;
        }

        public async Task<bool> RemoveAsync (int id)
        {
            OrderDetails orderDetails = await _context.OrderDetails.SingleAsync(d => d.Id == id);
            _context.OrderDetails.Remove(orderDetails);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<OrderDetails> GetOrderDetailsAsync(int id)
        {
            return await _context.OrderDetails.SingleAsync(d => d.Id == id);
        }

        public async Task<bool> UpdateOrderDetailsAsync(List<OrderDetails> detail)
        {
            foreach (var d in detail)
            {
                var det = await _context.OrderDetails.SingleAsync(o => o.Id == d.Id);
                _context.Entry(det).CurrentValues.SetValues(d);
            }           
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            //var address = await _context.add
            
            if (user.Address == null)
            {
                _context.Address.Add(user.Address);
                //user.Address. = new Address();
                
            }
            _context.Entry(user).State = EntityState.Modified;
           // user.Address.User = user;
            //await _context.SaveChangesAsync();
            // _context.Address.Update(user.Address);
            _context.Users.Update(user);
            //_context.Entry(result). .SetValues(user);
            //  _context.Entry(result.Address).CurrentValues.SetValues(user.Address);
            await _context.SaveChangesAsync();
            return user.Id;
        }


        public async Task<bool> UpdateOrderUserId(int orderId, int userId)
        {
            var order = await _context.Orders.SingleAsync(o => o.Id == orderId);
            var user = await _context.Users.SingleAsync(u => u.Id == userId);
            order.User = user;
            order.UserId = user.Id;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

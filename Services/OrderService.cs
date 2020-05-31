using Microsoft.EntityFrameworkCore;
using MotorGliding.Context;
using MotorGliding.Models.Db;
using MotorGliding.Models.ViewModels;
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
            return await _context.Orders.Include(d => d.OrderDetails).SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> GetPreviewAsync (int id)
        {
            return await _context.Orders.Include(d => d.OrderDetails).Include(u => u.OrderUser).ThenInclude(a => a.Address).SingleAsync(o => o.Id == id);
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
                det.Quantity = d.Quantity;
                //_context.Entry(det).CurrentValues.SetValues(d);
            }           
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<int> CreateUserAsync(EditUserViewModel user)
        {
            var newAddress = new Address
            {
                Street = user.Street,
                ZipCode = user.ZipCode,
                City = user.City,
                Country = user.Country
            };
            var newUser = new OrderUser()
            {
                Name = user.Name,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = newAddress,
                Email = user.Email,   
                OrderId = user.OrderId,
                UserId = user.Id != 0 ? user.Id : 0

            };
            _context.OrderUsers.Add(newUser);
            
            await _context.SaveChangesAsync();
            return newUser.Id;
        }

        public async Task<int> UpdateUserAsync(User user)
        {
            
            if (user.Address == null)
            {
                _context.Address.Add(user.Address);
                
            }
            _context.Entry(user).State = EntityState.Modified;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<bool> UpdateOrderUserId(int orderId, int userId)
        {
            var order = await _context.Orders.SingleAsync(o => o.Id == orderId);
            var userOrder = await _context.OrderUsers.SingleAsync(u => u.Id == userId);
            order.Accepted = true;
            order.OrderUserId = userOrder.Id;
          //  order.OrderUser = userOrder;
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> OrderAccept (int id)
        {
            var order = await _context.Orders.SingleAsync(o => o.Id == id);
            order.Accepted = true;
            order.CreateData = DateTime.Now;
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IList<Order>> GetSummaryOrders()
        {
            return await _context.Orders.Include(d => d.OrderDetails).Include(u => u.OrderUser).Where(o => o.Accepted).ToListAsync();
        }

        public async Task<IList<Order>> FilterOrderContainingEvent(int id)
        {
            var order = await _context.Orders.Include(d => d.OrderDetails.Where(e => e.EventID == id)).Include(u => u.OrderUser).Where(o => o.Accepted).ToListAsync();
            //order.Where(d => d.OrderDetails. Contains();
            return order;
        }

        public async Task<IList<Order>> FilterOrderByUser(int id)
        {
            return await _context.Orders.Include(d => d.OrderDetails).Include(u => u.OrderUser).Where(o => o.Accepted && o.OrderUser.UserId == id).ToListAsync();
        }
    }
}

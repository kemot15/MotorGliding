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
    public class AccountService : IAccountService
    {
        private readonly MotorGlidingContext _context;

        public AccountService(MotorGlidingContext context)
        {
            _context = context;
        }

        public async Task<Address> GetUserAddress(int id)
        {
            var address = await _context.Address.SingleOrDefaultAsync(a => a.User.Id == id);
            return address ?? null;
            //return await _context.Address.SingleAsync(a => a.User.Id == id);
        }
    }
}

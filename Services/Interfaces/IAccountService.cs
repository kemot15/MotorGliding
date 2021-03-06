﻿using MotorGliding.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Services.Interfaces
{
    public interface IAccountService
    {
        Task<Address> GetUserAddress(int id);
        Task<bool> UserLockAsync(int id);
        Task<User> GetUser(int id);
    }
}

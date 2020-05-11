using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.Db
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; }
    }
}

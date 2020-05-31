using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MotorGliding.Models.ViewModels
{
    public class EmailViewModel
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Email { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public int Phone { get; set; }
        public bool IsHtml { get; set; }
    }
}

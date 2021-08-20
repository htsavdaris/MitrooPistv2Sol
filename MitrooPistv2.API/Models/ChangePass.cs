using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MitrooPistv2.API.Models
{
    public class ChangePass
    {
        public string login { get; set; }
        public string oldpassword { get; set; }
        public string newpassword { get; set; }
    }
}

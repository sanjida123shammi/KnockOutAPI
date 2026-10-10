using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KnockOutAPI.Models.DTOs
{
    public class GuestRequest
    {
        public Guest Guest { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageFileName { get; set; }
    }
}
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreApi
{
    public class EventInputModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Time { get; set; }
        public IFormFile Photo { get; set; }
    }
}

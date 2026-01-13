using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Domain.Entities
{
    public class Client
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string lastname { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
    }
}

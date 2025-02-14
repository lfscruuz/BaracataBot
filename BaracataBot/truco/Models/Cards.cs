using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baracata.Truco.Models
{
    internal class Cards
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int value { get; set; }
        public string image { get; set; }
    }

    internal sealed class CardStructure
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int value { get; set; }
        public string image { get; set; }
    }
}

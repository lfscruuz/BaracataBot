using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baracata.Truco.Models
{
    internal class Cards
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public string Image {  get; set; }
    }
}

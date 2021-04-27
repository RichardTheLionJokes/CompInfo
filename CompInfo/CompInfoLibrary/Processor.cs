using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompInfoLibrary
{
    public class Processor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Frequency { get; set; }

        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompInfoLibrary
{
    public class RAM
    {
        public int Id { get; set; }
        public float Capacity { get; set; }
        public Int64 Speed { get; set; }

        public int ComputerId { get; set; }
        public virtual Computer Computer { get; set; }
    }
}

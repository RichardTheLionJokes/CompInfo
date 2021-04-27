using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompInfoLibrary
{
    public class Computer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string IP { get; set; }
        public string InvNumber { get; set; }
        public string OS { get; set; }
        public string Motherboard { get; set; }

        public virtual ICollection<Processor> Processors { get; set; }
        public virtual ICollection<RAM> RAMs { get; set; }
        public virtual ICollection<HardDisk> HardDisks { get; set; }
        public virtual ICollection<Printer> Printers { get; set; }
    }
}

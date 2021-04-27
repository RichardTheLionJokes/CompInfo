using System;
using System.Data.Entity;
using System.Linq;
using CompInfoLibrary;

namespace CompInfo.Models
{
    public class CompInfoContext : DbContext
    {
        public CompInfoContext()
            : base("name=CompInfoContext")
        {
        }

        public DbSet<Computer> Computers { get; set; }
        public DbSet<Processor> Processors { get; set; }
        public DbSet<RAM> RAMs { get; set; }
        public DbSet<HardDisk> HardDisks { get; set; }
        public DbSet<Printer> Printers { get; set; }
    }
}
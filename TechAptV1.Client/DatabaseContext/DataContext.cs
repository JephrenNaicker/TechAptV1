// Copyright © 2025 Always Active Technologies PTY Ltd

using Microsoft.EntityFrameworkCore;
using TechAptV1.Client.Models;

namespace TechAptV1.Client.DatabaseContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Number> Numbers { get; set; }
    }
}

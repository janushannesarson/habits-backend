using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using habitsbackend.Models;
using Microsoft.EntityFrameworkCore;

namespace habitsbackend.Data;

public class AppDbContext : DbContext
{
    public DbSet<Habit> Habits { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Entry> Entries { get; set; }


    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
               
    }
}

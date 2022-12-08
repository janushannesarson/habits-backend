using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace habitsbackend.Models;

public class Entry
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int HabitId { get; set; }
    
}

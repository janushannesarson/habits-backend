using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using habitsbackend.Models;
using Microsoft.AspNetCore.Mvc;
using habitsbackend.Data;
using Microsoft.EntityFrameworkCore;

namespace habitsbackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HabitsController : ControllerBase
{
    private static AppDbContext _context;
    private readonly ILogger<HabitsController> _logger;

    public HabitsController(AppDbContext context, ILogger<HabitsController> logger)
    {
        _context = context;
        _logger = logger;
    }



    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var habits = await _context.Habits.ToListAsync();
        return Ok(habits);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var habit = await _context.Habits.FirstOrDefaultAsync(x => x.Id == id);

        if(habit == null)
            return BadRequest("Bad request");
        
        return Ok(habit);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Habit habit)
    {
        await _context.AddAsync(habit);
        await _context.SaveChangesAsync();

        return CreatedAtAction("Get", habit.Id, habit);
    }

    [HttpPatch]
    public async Task<IActionResult> Patch(int id, string name)
    {
        var habit = await _context.Habits.FirstOrDefaultAsync(x => x.Id == id);

        if (habit == null)
            return BadRequest("Invalid id");
        
        habit.Name = name;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var habit = await _context.Habits.FirstOrDefaultAsync(x => x.Id == id);

        if (habit == null)
            return BadRequest("Invalid id");
        
        _context.Habits.Remove(habit);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

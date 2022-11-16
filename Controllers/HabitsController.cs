using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using habitsbackend.Models;
using Microsoft.AspNetCore.Mvc;
using habitsbackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace habitsbackend.Controllers;

[Route("[controller]")]
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

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Habit habit)
    {
        if(habit.Id != null)
            return BadRequest("Cannot specify habit id");
        
        string uid = User.Claims.ToList().First(x => x.Type == "id").Value;

        if(uid != habit.UserId)
            return BadRequest("Habit user id does not match authenticated user");


        User? user = await _context.Users.FirstOrDefaultAsync<User>(x => x.Id == habit.UserId);

        if (user is null)
            return BadRequest("Bad request");
      

        await _context.AddAsync(habit);
        await _context.SaveChangesAsync();    
        

        return CreatedAtAction("Get", habit.Id, habit);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string uid = User.Claims.ToList().First(x => x.Type == "id").Value;

        IList<Habit> habits = await _context.Habits.Where(x => 
            x.UserId == uid)
            .ToListAsync();

        if(habits.Count == 0)
            return NoContent();
        
        return Ok(habits);
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var habit = await _context.Habits.FirstOrDefaultAsync(x => x.Id == id);

        if(habit == null)
            return BadRequest("Bad request");
        
        string uid = User.Claims.ToList().First(x => x.Type == "id").Value;

        if(uid != habit.UserId)
            return BadRequest("Habit user id does not match authenticated user");


        return Ok(habit);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Habit habit)
    {
        if(habit.Id == null)
            return BadRequest("Missing habit id");

        string uid = User.Claims.ToList().First(x => x.Type == "id").Value;

        if(uid != habit.UserId)
            return BadRequest("Habit user id does not match authenticated user");

        User? user = await _context.Users.FirstOrDefaultAsync<User>(x => x.Id == habit.UserId);

        if (user is null)
            return BadRequest("Bad request");
        
        _context.Habits.Update(habit);
        await _context.SaveChangesAsync();

        return Ok(habit);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var habit = await _context.Habits.FirstOrDefaultAsync(x => x.Id == id);

        if (habit == null)
            return BadRequest("Invalid habit id");
        
        string uid = User.Claims.ToList().First(x => x.Type == "id").Value;

        if(uid != habit.UserId)
            return BadRequest("Invalid habit id");
        
        _context.Habits.Remove(habit);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

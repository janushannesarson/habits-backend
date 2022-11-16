using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using habitsbackend.Data;
using habitsbackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace habitsbackend.Controllers;

[Route("[controller]")]
public class UsersController : ControllerBase
{
    private AppDbContext _context;
    private readonly ILogger<UsersController> _logger;

    public UsersController(AppDbContext context, ILogger<UsersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Register()
    {
        string uid = User.Claims.First(x => x.Type == "id").Value;
        User? user = await _context.Users.FirstOrDefaultAsync<User>(x => x.Id == uid);     

        if(user is not null)
        {
            return Ok("Already registered");
        }

        // Register user ...
        user = new User () {
            Id = uid,
            Name = User.Claims.First(x => x.Type == "name").Value,
            Email = User.Claims.First(x => x.Type == "email").Value
        };
        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok("Registered " + user.Email);
    }

}
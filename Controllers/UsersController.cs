using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FirebaseAdmin.Auth;
using habitsbackend.Data;
using habitsbackend.Models;
using Microsoft.EntityFrameworkCore;

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

    [HttpGet("{id:int}")]
    public async Task<ActionResult> Get(string id)
    {
        User? user = await _context.Users.FirstOrDefaultAsync<User>(x => x.Id == id);

        if(user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult> Register(string idToken)
    {
        FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        string uid = decodedToken.Uid;

        User? user = await _context.Users.FirstOrDefaultAsync<User>(x => x.Id == uid);     

        if(user is not null)
        {
            return Ok("Already registered");
        }

        // Register user ...
        return Ok();
    }

}
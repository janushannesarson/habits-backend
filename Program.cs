using Google.Apis.Auth.OAuth2;
using habitsbackend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using habitsbackend.Authentication;
using FirebaseAdmin;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

byte[] data = Convert.FromBase64String(Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS"));
string decodedString = System.Text.Encoding.UTF8.GetString(data);

builder.Services.AddSingleton(FirebaseAdmin.FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromJson(decodedString)
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddScheme<AuthenticationSchemeOptions, FirebaseAuthenticationHandler>(JwtBearerDefaults.AuthenticationScheme, (opt) => 
    {

    });

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    await next();
});

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ProductManager.Api.Extensions;
using ProductManager.Application.DTOs.Configuration;
using ProductManager.Data.Context;
using ProductManager.IoC.Container;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.UseIoC(builder.Configuration);
builder.Services.Configure<JwtConfigurationDto>(builder.Configuration.GetSection("JWT"));


#region Identity

builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ProductManagerContext>()
    .AddDefaultTokenProviders();

#endregion

#region Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
    };
});

#endregion

var app = builder.Build();

#region Auto Migrate

app.MigrateDatabase<ProductManagerContext>((context, services) =>
{
    var logger = services.GetService<ILogger<ProductManagerContextSeed>>();
    if (logger != null) ProductManagerContextSeed.SeedAsync(context, logger).Wait();
});

#endregion
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

app.Run();

using System.Text;
using GistBlog.BLL.Services.Contracts;
using GistBlog.BLL.Services.Implementation;
using GistBlog.DAL.DbConfig;
using GistBlog.DAL.Entities.Models.Domain;
using GistBlog.DAL.Repository.Contracts;
using GistBlog.DAL.Repository.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Description = "Standard Authorization Header Using the Bearer Scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

#region Database connection configuration

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#endregion

#region Identity Configuration

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

#endregion

#region Jwt configuration

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:ValidAudience"],
            ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
        };
    });

#endregion

#region Register token service

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<DataContext>>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region Enable Cors and Authentication header

app.UseCors(options =>
{
    options.WithOrigins("*")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
});

app.UseAuthentication();

#endregion

app.UseAuthorization();

app.MapControllers();

app.Run();

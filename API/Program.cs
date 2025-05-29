using API.DataBase;
using API.Helppers.Mapper;
using API.Repositories;
using API.Repositories.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Mapper
var config = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperDTO()); });
IMapper mapper = config.CreateMapper();

//JWT
var jwtConfig = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtConfig["Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtConfig["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});
builder.Services.AddAuthorization();

//CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowFrontend", policy => { policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod(); });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new() { Title = "Advance Request API", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {token}"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

//builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddDbContext<DesafioContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IAdvanceRequestRepository, AdvanceRequestRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Advance Request API v1");
        opt.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

//app.UseMvc();
app.UseRouting();
app.UseCors("AllowFrontend");
//app.UseCors(opt => { opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
app.UseAuthentication();
app.UseAuthorization();
app.UseStatusCodePages();

app.UseEndpoints(e => { e.MapControllers(); });

app.Run();

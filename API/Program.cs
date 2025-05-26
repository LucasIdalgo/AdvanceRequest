using API.DataBase;
using API.Helppers.Mapper;
using API.Repositories;
using API.Repositories.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var config = new MapperConfiguration(cfg => { cfg.AddProfile(new MapperDTO()); });
IMapper mapper = config.CreateMapper();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddDbContext<DesafioContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(mapper);

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IAdvanceRequestRepository, AdvanceRequestRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePages();
app.UseMvc();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

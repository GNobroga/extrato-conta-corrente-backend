using AutoMapper;
using backend.Data;
using backend.Extensions;
using backend.Models;
using backend.Services;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("AppConnection"));
});

var mapperConfig = new MapperConfiguration(cfg => {
    cfg.CreateMap<Lancamento, LancamentoDTO>();
    cfg.CreateMap<LancamentoDTO, Lancamento>();
});

var mapper = new Mapper(mapperConfig);

builder.Services.AddControllers();

builder.Services.AddSingleton<IMapper>(mapper);

builder.Services.AddScoped<ILancamentoService, LancamentoServiceImpl>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseCors(cors => cors.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.MapControllers();

app.Run();
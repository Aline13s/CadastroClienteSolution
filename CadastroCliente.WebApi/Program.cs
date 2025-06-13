using CadastroCliente.Application.Interfaces;
using CadastroCliente.Application.Services;
using CadastroCliente.Application.Validators;
using CadastroCliente.Domain.Interfaces;
using CadastroCliente.Infrastructure.Data;
using CadastroCliente.Infrastructure.Mappings;
using CadastroCliente.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("http://localhost:4200") // frontend Angular
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // opcional, se estiver usando cookies
    });
});

// Add services to the container.

builder.Services.AddControllers();

// 2) Registra o explorador de endpoints e o gerador de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CadastroClientes API",
        Version = "v1",
        Description = "API para gerenciar Clientes e Menu dinâmico"
    });
});

// AutoMapper & FluentValidation
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<ClienteValidator>();
builder.Services.AddFluentValidationAutoValidation();

// Repositórios e UoW
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Serviços
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IMenuService, MenuService>();

var app = builder.Build();

// 3) Habilita Swagger apenas em Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CadastroClientes API V1");
        c.RoutePrefix = "swagger"; // acesso em https://localhost:xxxx/swagger
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

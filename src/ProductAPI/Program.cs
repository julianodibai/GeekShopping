using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductAPI.Infra.Context;
using ProductAPI.Infra.Repository;
using ProductAPI.Models;
using ProductAPI.Services.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

#region Dependency Injection

builder.Services.AddScoped<IProductRepository, ProductRepository>();

#endregion

#region AutoMapper

var autoMapperConfig = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                });

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

#region Database

var connection = builder.Configuration["MySQlConnection:MySQlConnectionString"];

builder.Services.AddDbContext<ProductContext>(options => options.
            UseMySql(connection,
                        new MySqlServerVersion(
                            new Version(8, 0, 5))));
#endregion

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekShopping.ProductAPI v1"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

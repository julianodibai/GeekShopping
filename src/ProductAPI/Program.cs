using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductAPI.Infra.Context;
using ProductAPI.Infra.Repository;
using ProductAPI.Models;
using ProductAPI.Services.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(opt =>
    {
        opt.Authority = "https://localhost:4435/";
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
        };
    });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("ApiScope", p =>
    {
        p.RequireAuthenticatedUser();
        p.RequireClaim("scope", "geek_shopping");
    });
});

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekShoopping.ProductAPI", Version = "v1" });
    s.EnableAnnotations();
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"Enter 'Bearer' [space] and your token!",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        } 
    }); 
});

#region Dependency Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();

#endregion

#region Database

var connectionString = builder.Configuration.GetConnectionString("ProductAPI");

builder.Services.AddDbContext<ProductContext>(options => options
        .UseSqlServer(connectionString),
        ServiceLifetime.Transient
        );

#endregion

#region AutoMapper

var autoMapperConfig = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                        cfg.CreateMap<Product, ProductAddDTO>().ReverseMap();
                    });

builder.Services.AddSingleton(autoMapperConfig.CreateMapper());

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion

var app = builder.Build();

app.UseDeveloperExceptionPage();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekShopping.ProductAPI v1"));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

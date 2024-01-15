using CourierCompanyApi;
using CourierCompanyApi.Authentication;
using CourierCompanyApi.Data;
using CourierCompanyApi.Repository;
using CourierCompanyApi.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option => {
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddResponseCaching();
//builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddControllers(/*x => x.Filters.Add<ApiKeyAuthFilter>()*/)
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "The API Key to access the API",
        Type = SecuritySchemeType.ApiKey,
        Name = "x-api-key",
        In = ParameterLocation.Header,
        Scheme = "ApiKeyScheme"
    });
    var scheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header,
    };
    var requirement = new OpenApiSecurityRequirement
    {
        { scheme, new List<string>() }
    };
    c.AddSecurityRequirement(requirement);
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml")); 
});

builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddScoped<SpecialApiKeyAuthFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

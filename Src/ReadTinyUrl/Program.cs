using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using ReadTinyUrl.Models;
using Shared.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExportedServices();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddSingleton(service => new MongoClient(builder.Configuration.GetConnectionString("TinyUrl")));

builder.Services.AddSingleton(service =>
{
    var options = new ConfigurationOptions
    {
        EndPoints =
        {
            { 
                builder.Configuration.GetConnectionString("Redis:Host")!, 
                builder.Configuration.GetValue<int>("ConnectionStrings:Redis:Port")
            }
        }
    };
    var cluster = ConnectionMultiplexer.Connect(options);
    return cluster.GetDatabase();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var identityServiceSettings = builder.Configuration.GetSection("IdentityService").Get<IdentityServiceSettings>()!;
        options.Authority = identityServiceSettings.Authority;
        options.ClaimsIssuer = identityServiceSettings.Authority;
        options.Audience = identityServiceSettings.ClientId;
        options.RequireHttpsMetadata = false;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

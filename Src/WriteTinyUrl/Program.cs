using WriteTinyUrl;
using Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WriteTinyUrl.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddDbContext<UrlRangeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UrlRangeContext")!));
builder.Services.AddTransactionContext<UrlRangeContext>();

builder.Services.AddSingleton(services => new MongoClient(builder.Configuration.GetConnectionString("TinyUrl")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var identityServiceSettings = builder.Configuration.GetSection("IdentityService").Get<IdentityServiceSettings>()!;
        options.Authority = identityServiceSettings.Authority;
        options.ClaimsIssuer = identityServiceSettings.Authority;
        options.Audience = identityServiceSettings.ClientId;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddExportedServices();

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


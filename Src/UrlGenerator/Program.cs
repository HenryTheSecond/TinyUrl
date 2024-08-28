using Microsoft.EntityFrameworkCore;
using UrlGenerator;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UrlRangeContext>(options => {
    options.UseSqlServer(a);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (builder.Configuration.GetValue<bool>("DisplaySwagger"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

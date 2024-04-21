using System.Text.Json.Serialization;
using Leanwork.Rh.API.Extension;
using Leanwork.Rh.Application.Mapping;
using Leanwork.Rh.CrossCutting;
using Leanwork.Rh.Domain.Interface;
using Leanwork.Rh.Infrastructure.DbAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.LayerDependency();
builder.Services.AddAutoMapper(typeof(AMConfiguration));
builder.Services.AddSwaggerGen();

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

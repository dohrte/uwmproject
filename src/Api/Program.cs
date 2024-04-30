using Api;
using Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddScoped<IRepository<Automobile>, AutomobileRepository<Automobile>>();
builder.Services.AddScoped<AutomobileService>();

builder.Services.AddSingleton<HttpClient>(s => {
  return new HttpClient{
    BaseAddress = new Uri("https://pokeapi.co/api/v2/")
  };
});
builder.Services.AddScoped<PokeApiService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

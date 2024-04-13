using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CoursesAPI.Data;
using Plain.RabbitMQ;
using RabbitMQ.Client;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CoursesAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CoursesAPIContext") ?? throw new InvalidOperationException("Connection string 'CoursesAPIContext' not found.")));

Console.WriteLine("(courses) conn string: " + builder.Configuration["MessageBroker:Host"]);

builder.Services.AddSingleton<IConnectionProvider>(
    new ConnectionProvider(builder.Configuration["MessageBroker:Host"]));
builder.Services.AddScoped<IPublisher>(x => new Publisher(x.GetService<IConnectionProvider>(), "enrollment_exchange", ExchangeType.Topic));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();

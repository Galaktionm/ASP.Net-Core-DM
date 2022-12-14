using Microsoft.EntityFrameworkCore;
using ScrantonBranch;
using ScrantonBranch.Entities;
using ScrantonBranch.Services;
using Consul;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
 options.AddPolicy(name: "AngularPolicy",
 cfg => {
     cfg.AllowAnyHeader();
     cfg.AllowAnyMethod();
     cfg.WithOrigins(builder.Configuration["AllowedCORS"]);
 }));

builder.Services.AddHealthChecks();

builder.Services.AddSingleton<MessageReceiver>();



var app = builder.Build();

app.UseHealthChecks("/api/health");

app.UseCors("AngularPolicy");


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    Product prod = new Product("White Paper", 30, 300);
    OrderProduct op = new OrderProduct(prod, 30);
    List<OrderProduct> orders = new List<OrderProduct>();
    orders.Add(op);
    Order order = new Order("TestClient", orders);
    Order order2 = new Order("LOLOL", orders);

    dbContext.Add(prod);
    dbContext.Add(order);
    dbContext.Add(order2);
    dbContext.SaveChanges();

}

/*
using (var scope = app.Services.CreateScope())
{
    var receiver = scope.ServiceProvider.GetService<MessageReceiver>();
    MessageReceiver.addInterestedProduct("White Paper A4");
    receiver.receiveMessages();
}
*/
var receiver=app.Services.GetService<MessageReceiver>();
MessageReceiver.addInterestedProduct("White Paper A4");
receiver.receiveMessages();


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
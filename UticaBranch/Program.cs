using Microsoft.EntityFrameworkCore;
using UticaBranch;
using UticaBranch.Services;
using UticaBranch.Entities;

var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);



// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddSingleton<MessageReceiver>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetService<DatabaseContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();


}

using (var scope = app.Services.CreateScope())
{
    var receiver = scope.ServiceProvider.GetService<MessageReceiver>();
    MessageReceiver.addInterestedProduct("White Paper A4");
    receiver.receiveMessages();
}

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

using BookStoreApi.Services;
using Microsoft.EntityFrameworkCore;
using ProductService;
using ProductService.Entities;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
 options.AddPolicy(name: "AngularPolicy",
 cfg => {
     cfg.AllowAnyHeader();
     cfg.AllowAnyMethod();
     cfg.WithOrigins(builder.Configuration["AllowedCORS"]);
 }));

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabase"));


builder.Services.AddSingleton<MessageSender>();
builder.Services.AddSingleton<DatabaseService>();


builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{

    var dbContext = scope.ServiceProvider.GetService<DatabaseService>();

    Product product1 = new Product("White Paper A4", 20, 300, "500 sheets in one box", "RandomCompany1");
    Product product2 = new Product("White Paper A4 Premium", 35, 200, "500 sheets in one box", "RandomCompany1");
    Product product3 = new Product("Colored paper bundle, 40 sheets", 20, 200, "40 sheets, 8 different colors (5 each)", "ColoredPaperCompany");
    Product product4 = new Product("Colored paper bundle, 800 sheets", 50, 150, "800 sheets, 8 different colors (100 each)", "ColoredPaperCompany");

    await dbContext.DropAll();
    await dbContext.CreateAsync(product1);
    await dbContext.CreateAsync(product2);
    await dbContext.CreateAsync(product3);
    await dbContext.CreateAsync(product4);
}


app.UseCors("AngularPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
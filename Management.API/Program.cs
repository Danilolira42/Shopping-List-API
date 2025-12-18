using Management.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ProductDbContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    
})
;

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();


app.MapControllers();
app.UseAuthorization();
app.MapFallbackToFile("index.html");


app.Run();

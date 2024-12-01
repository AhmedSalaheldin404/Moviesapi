using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moviesapi.Models;

var builder = WebApplication.CreateBuilder(args);
// هنا بضيف كونيكشن سترينج
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"); //بحط عنوان الكونيكشن سترينج الي في appstettings
builder.Services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(ConnectionString)); // بعدين كدا بضيف داتابيزكونتيكست وابعتله الكونيكشن سترينج في شكل متغير
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

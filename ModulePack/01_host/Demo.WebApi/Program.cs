using Demo.WebApi;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//app.UseAuthorization();

//app.MapControllers();

//app.Run();




var builder = WebApplication.CreateBuilder(args);

Startup startup = new Startup();
startup.ConfigureServices(builder.Services);



var app = builder.Build();




startup.Configure(app);
app.Run();
using basicMongoDBandMiddleware.Middleware;
// using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();


app.UseMiddleware<CapitalizeMiddleware>();
app.UseMiddleware<LowercaseMiddleware>();


app.MapControllers();

app.Run();
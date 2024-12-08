using WebApplication1;



var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure()
    .AddSwagger()
    .AddApplicationServices()
    .AddControllers();

    
var app = builder.Build();

app.UseExceptionHandler("/error");

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
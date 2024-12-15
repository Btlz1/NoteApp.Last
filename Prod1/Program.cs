using btlz.Composer;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddConfiguredSwagger()
    .AddJwt()
    .AddAuth(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddSwagger()
    .AddApplicationServices()
    .AddControllers();
    
    


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); 
app.UseAuthorization(); 
app.MapControllers(); 

app.Run();
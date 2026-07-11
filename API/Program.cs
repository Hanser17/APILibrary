using API.MiddleWares;
using API.SwaggerDocumentation;
using Application.ServicesRegistration;
using Identity.IdentityRegistration;

using Persistance.RepoRegistration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceDependencies(builder.Configuration);
builder.Services.AddServiceRegistration();
builder.Services.AddSwaggerDocumentation(); 
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();
await app.Services.RunSeeds();
await app.Services.EntityRunSeeds();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
public partial class Program
{

}
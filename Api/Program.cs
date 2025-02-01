using Api.Extensions;
using Api.Filters;
using Api.Middlewares;
using Application.Extensions;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => options.Filters.Add(typeof(RequestFilterAttribute)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationLayer();
builder.Services.AddContextInfrastructure(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddEssentials();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc(o => { var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); o.Filters.Add(new AuthorizeFilter(policy)); });

var app = builder.Build();
app.ConfigureSwagger();
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        if (!roleManager.Roles.Any())
        {
            await Infrastructure.Identity.Seeds.DefaultRoles.SeedAsync(roleManager);
            Log.Information("Finalizó la inserción de datos por defecto");
            Log.Information("Iniciando aplicación");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Ocurrió un error al insertar los datos por defecto");
    }
}

await app.RunAsync();


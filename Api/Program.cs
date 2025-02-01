using Api.Extensions;
using Api.Filters;
using Api.Middlewares;
using Application.Extensions;
using Infrastructure.DbContext;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
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
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

await app.RunAsync();


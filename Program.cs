using management_webapp_bn.Data;
using management_webapp_bn.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular app URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromSeconds(2520)); // Cache preflight for 42 minutes
    });

    // Policy ?????? development (?????????)
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .SetPreflightMaxAge(TimeSpan.FromSeconds(2520))
              .WithExposedHeaders("*"); // Expose all headers
    });
});

// Add services db
builder.Services.AddDbContext<AppDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ??? HTTPS redirection ?????? development ???????????? 307 redirect
// app.UseHttpsRedirection(); // Comment out for development

// ????? CORS middleware ???? UseAuthorization
// ??? "AllowAll" ?????? development ???? "AllowAngularApp" ?????? production
app.UseCors("AllowAll"); // ?????? development
// app.UseCors("AllowAngularApp"); // ?????? production

app.UseAuthorization();

app.MapControllers();

app.Run();
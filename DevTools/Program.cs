using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyWebAPI.data;
using MyWebAPI.Repositories;
using MyWebAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MyWebAPI.Services.Interfaces;
using MyWebAPI.Services;
using DevTools.Services;
// using Services.BackgroundServices.PluginsWatchers;
using DevTools.Services.Interfaces;
using DevTools.Repositories.Interfaces;
using DevTools.data;
using DevTools.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(c => {
//     c.SwaggerDoc("v1", new OpenApiInfo {Title = "MyWebApi", Version = "v1"});
// });

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddDbContext<MyDbContext>(op => 
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlServerOptions => sqlServerOptions.CommandTimeout(300));
});

builder.Services.AddIdentity<User, IdentityRole>(options => 
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
}).AddEntityFrameworkStores<MyDbContext>();
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = 
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme = 
    options.DefaultScheme = 
    options.DefaultSignInScheme = 
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => 
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigninKey"]))
    };
});

//  services
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountManagerService, AccountManagerService>();
builder.Services.AddScoped<IPluginManagerService, PluginManagerService>();

//  repositories
builder.Services.AddScoped<IPluginCategoryRepository, PluginCategoryRepository>();
builder.Services.AddScoped<IPluginManagerRepository, PluginManagerRepository>();
builder.Services.AddScoped<IPluginRepository, PluginRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

// Background Services
builder.Services.AddHostedService<PluginWatcherService>(); // Thêm vào Hosted Services

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var pluginManagerService = scope.ServiceProvider.GetRequiredService<IPluginManagerService>();
    await pluginManagerService.LoadPlugins();
}

// var pluginWatcher = app.Services.GetRequiredService<PluginWatcher>();
// pluginWatcher.StartWatching();

app.Run();


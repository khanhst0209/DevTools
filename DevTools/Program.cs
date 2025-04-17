using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyWebAPI.data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MyWebAPI.Services.Interfaces;
using MyWebAPI.Services;
using DevTools.Services;
// using Services.BackgroundServices.PluginsWatchers;
using DevTools.Services.Interfaces;
using DevTools.Repositories.Interfaces;
using DevTools.Repositories;
using DevTools.Helper.Mapper;
using DevTools.data.Seed;
using Services.AssemblyManager;
using DevTools.Application.Services.Interfaces;
using DevTools.Application.Services;
using DevTools.Infrastructure.Repositories.Interfaces;
using DevTools.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Thêm dịch vụ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin() // Cho phép tất cả các domain
               .AllowAnyMethod() // Cho phép tất cả các phương thức HTTP (GET, POST, PUT, DELETE,...)
               .AllowAnyHeader(); // Cho phép tất cả các header
    });
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "DevTools", Version = "v1" });
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
    options.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<MyDbContext>();
builder.Services.AddAuthentication(options =>
{
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
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountManagerService, AccountManagerService>();
builder.Services.AddScoped<IPluginManagerService, PluginManagerService>();
builder.Services.AddScoped<IPluginCategoryService, PluginCategoryService>();
builder.Services.AddScoped<IUserPluginService, UserPluginService>();
builder.Services.AddScoped<IPluginLoader, PluginLoader>();
builder.Services.AddScoped<ISharedLibraryLoader, SharedLibraryLoader>();
builder.Services.AddScoped<IAssemblyManager, AssemblyManager>();
builder.Services.AddScoped<IPremiumUpgradeRequestService, PremiumUpgradeRequestService>();

//  repositories
builder.Services.AddScoped<IPluginCategoryRepository, PluginCategoryRepository>();
builder.Services.AddScoped<IPluginManagerRepository, PluginManagerRepository>();
builder.Services.AddScoped<IPluginRepository, PluginRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserPluginRepository, UserPluginRepository>();
builder.Services.AddScoped<IPremiumUpgradeRequestRepository, PremiumUpgradeRequestRepository>();

// Background Services
// builder.Services.AddHostedService<PluginWatcherService>(); // Thêm vào Hosted Services
// builder.Services.AddHostedService<LibraryWatcherService>(); // Thêm vào Hosted Services



//modelmapper
builder.Services.AddAutoMapper(typeof(UserProfile));

var app = builder.Build();

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleSeedData.SeedRolesAsync(services);
    await UserSeedData.SeedUsersAsync(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var pluginLoaderService = scope.ServiceProvider.GetRequiredService<IPluginLoader>();
    await pluginLoaderService.LoadPluginsAsync();

    // var libraryLoaderService = scope.ServiceProvider.GetRequiredService<ISharedLibraryLoader>();
    // await libraryLoaderService.LoadLibraryAsync();
}


app.Run();


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PetAdoption.Api.Data;
using PetAdoption.Api.Hubs;
using PetAdoption.Shared;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = TokenService.GetTokenValidationParameters(builder.Configuration);
        options.RequireHttpsMetadata = false; // Add this line
        options.SaveToken = true; // Add this line
        options.TokenValidationParameters.ValidateIssuer = false; // Add this line
        options.TokenValidationParameters.ValidateAudience = false; // Add this line
        options.TokenValidationParameters.ValidateLifetime = true; // Add this line
        options.TokenValidationParameters.ValidateIssuerSigningKey = true; // Add this line
        options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])); // Add your secret key here
    });

builder.Services.AddSwaggerGen(opt =>
{
    var securitySchema = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    opt.AddSecurityDefinition("Bearer", securitySchema);
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securitySchema, new[] { "Bearer" } }
    });
});

builder.Services.AddDbContext<PetContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Pet")), ServiceLifetime.Transient);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<IPetService, PetService>();
builder.Services.AddTransient<IUserPetService, UserPetService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserFriendService, UserFriendService>();
builder.Services.AddTransient<IMessageService, MessageService>();

builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    ApplyDbMigrations(app.Services);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

app.MapControllers();

app.MapHub<PetHub>(AppConstants.PetHubPattern);
app.MapHub<ChatHub>(AppConstants.ChatHubPattern);

app.Run("http://localhost:7055");

static void ApplyDbMigrations(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PetContext>();
    if (context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}
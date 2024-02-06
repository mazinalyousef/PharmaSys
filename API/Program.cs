using System.Text;
using System.Text.Json.Serialization;
using API.BusinessLogic;
using API.Data;
using API.Entities;
using API.Helpers;
using API.Hubs;
using API.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddDbContext<DataContext>(options=>
{
    options.UseSqlServer(

        builder.Configuration.GetConnectionString("DefaultConnection")
    );
});

builder.Services.AddIdentity<User,IdentityRole>()
.AddEntityFrameworkStores<DataContext>()
.AddDefaultTokenProviders();


// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };

    options.Events =new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accesstoken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accesstoken)&&path.StartsWithSegments("/hubs"))
            {
                context.Token = accesstoken;
            }
            return Task.CompletedTask;
        }
    };

});



//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


// add repository ....
builder.Services.AddScoped<IBatchRepository,BatchRepository>();
builder.Services.AddScoped<ITaskRepository,TaskRepository>();
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped<IBatchBL,BatchBL>();


builder.Services.AddSignalR();


var app = builder.Build();


// TODO: refactor the code .. later to transfer to another layer....
#region seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] 
    {
            UserRoles.Manager,
            UserRoles.Warehouse_CheckRooms,
            UserRoles.Warehouse_RawMaterials,
            UserRoles.QA_RawMaterials,
            UserRoles.QA_CheckEquipements,
            UserRoles.QA_Sampling,
            UserRoles.Accountant,
            UserRoles.Production_CheckEquipements,
            UserRoles.Production_Manufacturing,
            UserRoles.Production_Manager,
             UserRoles.Filling_CheckEquipements,
            UserRoles.Filling_FillingTubes,
            UserRoles.Filling_Cartooning,
            UserRoles.Filling_Packaging,
           UserRoles.Developer,
              
         
   };
 
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x=>x.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
.WithOrigins("https://localhost:4200"));

// added..... 
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("hubs/notification");

app.Run();

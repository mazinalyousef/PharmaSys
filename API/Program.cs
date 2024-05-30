using System.Text;
using System.Text.Json.Serialization;
using API.BusinessLogic;
using API.Data;
using API.Entities;
using API.Helpers;
using API.Hubs;
using API.Interfaces;
using API.Middleware;
using API.Timers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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

builder.Services.AddIdentity<User,IdentityRole>(opt=>
{
    opt.Password.RequiredLength=3;
    opt.Password.RequireDigit=false;
    opt.Password.RequiredUniqueChars=0;
    opt.Password.RequireUppercase=false;
    opt.Password.RequireNonAlphanumeric=false;
})
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

// testing
 builder.Services.AddAuthorization(opt=>
 {
    opt.AddPolicy("ManagerPolicy",policy=>policy.RequireRole(UserRoles.Manager,UserRoles.Developer));
 });



var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);


//builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(x=>x.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



builder.Services.AddScoped<IGlobalDataRepository,GlobalDataRepository>();

// add repository ....
builder.Services.AddScoped<IBatchRepository,BatchRepository>();
builder.Services.AddScoped<ITaskRepository,TaskRepository>();
builder.Services.AddScoped<INotificationRepository,NotificationRepository>();
builder.Services.AddScoped<IMessageRepository,MessageRepository>();
builder.Services.AddScoped<IBatchBL,BatchBL>();
builder.Services.AddScoped<ITaskBL,TaskBL>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.AddScoped<IEmailSender,EmailSender>();




builder.Services.AddSignalR();
builder.Services.AddScoped<TaskTimer>();
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


// try to acccess singletone service 
    //  var globalDataService= app.Services.GetRequiredService<IGlobalDataRepository>();
    //  globalDataService.getTaskTypeTimersData();
     var globalDataService = scope.ServiceProvider.GetRequiredService<IGlobalDataRepository>();
      bool dataReadCompleted=  globalDataService.getTaskTypeTimersData();

    
}




#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors(x=>x.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
.WithOrigins("https://localhost:4200"));

// added..... 
app.UseAuthentication();

app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles(); 

 // for the wwwroot/uploads folder
  string uploadsDir = Path.Combine(builder.Environment.WebRootPath, "uploads");
  if (!Directory.Exists(uploadsDir))
      Directory.CreateDirectory(uploadsDir);

  app.UseStaticFiles(new StaticFileOptions()
  {
      RequestPath = "/api/images",
      FileProvider = new PhysicalFileProvider(uploadsDir)
  }); 


app.MapControllers();


app.MapHub<NotificationHub>("hubs/notification");
app.MapHub<TaskTimerHub>("hubs/taskTimer");
app.MapHub<TaskReminderHub>("hubs/taskReminder");
app.MapHub<MessageHub>("hubs/message");

app.MapFallbackToController("Index","FallBack");
app.Run();

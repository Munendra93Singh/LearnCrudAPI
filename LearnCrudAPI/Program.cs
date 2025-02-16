using AutoMapper;
using DocumentFormat.OpenXml;
using LearnCrudAPI.Container;
using LearnCrudAPI.Helper;
using LearnCrudAPI.Model;
using LearnCrudAPI.Repos;
using LearnCrudAPI.Repos.Models;
using LearnCrudAPI.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRoleService, UserRoleService>();
builder.Services.AddTransient<IRefreshHandler, RefreshHandler>();
builder.Services.AddDbContext<LearndataContext>(o =>
o.UseSqlServer(builder.Configuration.GetConnectionString("apicon")));

builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions,BasicAuthenticationHanldler>("BasicAuthentication",null);

var _authkey = builder.Configuration.GetValue<string>("JwtSettings:securitykey");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

});



var automapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);


//Cors Policy
builder.Services.AddCors(p => p.AddPolicy("corsepolicy", build =>
{
    build.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
}));
//Cors Policy
builder.Services.AddCors(p => p.AddPolicy("corsepolicy1", build =>
{
    build.WithOrigins("https://domain3.com").AllowAnyHeader().AllowAnyMethod();
}));
//Cors Policy
builder.Services.AddCors(p => p.AddDefaultPolicy(build =>
{
    build.WithOrigins("*").AllowAnyHeader().AllowAnyMethod();
}));

//Rate limiting
builder.Services.AddRateLimiter(o => o.AddFixedWindowLimiter(policyName: "fixedwindow", options =>
{
    options.Window = TimeSpan.FromSeconds(10);
    options.PermitLimit = 1;
    options.QueueLimit = 0;
    //options.Window =TimeSpan.FromSeconds(1);
    options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode=401);

//==Logfile
string logpath = builder.Configuration.GetSection("Logging:Logpath").Value;
var _logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Information()
    .MinimumLevel.Override("microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logpath)
    .CreateLogger();
builder.Logging.AddSerilog(_logger);

//===JWTKey
var _jwtsetting = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JWTSettings>(_jwtsetting);

var app = builder.Build();

//==============Start Mimimal Api =======
app.MapGet("/minimalapi", () => "Nihira Techiees");

//app.MapGet("/getchannel", (string channelname) => "Welcome to " + channelname).WithOpenApi(opt =>
//{
//    var parameter = opt.Parameters[0];
//    parameter.Description = "Enter Channel Name";
//    return opt;
//});

//app.MapPost("/todayaction/{id}", async (string id) =>
//{
//    return Results.Created($"/todayaction/{id}", id);
//}).WithOpenApi(genooptions =>
//{
//    var parameter = genooptions.Parameters[0];
//    parameter.Description = "Enter Input";
//    return genoptions;
//});
app.MapGet("/country", (string[] channel) => $"tagl {channel[0]} &tag2 {channel[1]}");



app.MapGet("/getcustomer", async (LearndataContext db) => {
    return await db.TblCustomers.ToListAsync();
});

app.MapGet("/getcustomerbycode/{code}", async (LearndataContext db, string code) => {
    return await db.TblCustomers.FindAsync(code);
});

app.MapPost("/createcustomer", async (LearndataContext db, TblCustomer customer) => {
    await db.TblCustomers.AddAsync(customer);
    await db.SaveChangesAsync();
});

app.MapPut("/updatecustomer/{code}", async (LearndataContext db, TblCustomer customer, string code) => {
    var existdata = await db.TblCustomers.FindAsync(code);
    if (existdata != null)
    {
        existdata.Name = customer.Name;
        existdata.Email = customer.Email;
    }
    await db.SaveChangesAsync();
});

app.MapDelete("/removecustomer/{code}", async (LearndataContext db, string code) => {
    var existdata = await db.TblCustomers.FindAsync(code);
    if (existdata != null)
    {
        db.TblCustomers.Remove(existdata);
    }
    await db.SaveChangesAsync();
});
//app.MapPost("/upload", async (IFormFile file) =>
//{
//    string filename = "Upload/" + file.FileName;
//    using var stream = File.OpenWrite(filename);
//    await file.CopyToAsync(stream);
//});

//app.MapPost("/upload", async (IFormFileCollection collection) =>
//{
//   foreach(var file in collection)
//    {
//        string filename = "Upload/" + file.FileName;
//        using var stream = File.OpenWrite(filename);
//        await file.CopyToAsync(stream);
//    }
//});

//====End minimal Api

//Rate Limite==
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

//===MiddleWare Policy==
app.UseCors();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

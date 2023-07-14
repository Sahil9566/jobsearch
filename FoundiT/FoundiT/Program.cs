using Application.Repository.IRepository;
using Application.Repository;
using FoundiT.DTOMapping;
using Infastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Azure.Storage.Blobs;
using Twilio;
using Twilio.Rest.Verify.V2.Service;
using Microsoft.WindowsAzure.Storage;
using FluentValidation;
using Domain.DTOs;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string cs = builder.Configuration.GetConnectionString("cnStr");
builder.Services.AddDbContext<Applicationdbcontext>(options => options.UseSqlServer(cs));
builder.Services.AddScoped(_ =>
{
    return new BlobServiceClient(builder.Configuration.GetConnectionString("AzureStorage"));
});



string accountSid = Environment.GetEnvironmentVariable("ACd9245b36f4ad0c28d71852ac7632237c");
string authToken = Environment.GetEnvironmentVariable("2b767e230a8524084729197666ec5b98");

var connectionString = builder.Configuration.GetConnectionString("AzureStorage");
builder.Services.AddScoped(_ =>
{
    var storageAccount = CloudStorageAccount.Parse(connectionString);
    var blobClient = storageAccount.CreateCloudBlobClient();
    var tableClient = storageAccount.CreateCloudTableClient();

    return new
    {
        BlobClient = blobClient,
        TableClient = tableClient
    };
});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//            .AddGoogle(options =>
//            {
//                options.ClientId = "[MyGoogleClientId]";
//                options.ClientSecret = "[MyGoogleSecretKey]";
//            });



//builder.Services.AddAuthentication().AddGoogle(googleOptions =>
//{
//    googleOptions.ClientId = builder.Configuration["Authentication:Google:clientid"];
//    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:clientsecret"];
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyCors", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
builder.Services.AddScoped<ISmsRepository, SmsRespository>();
object value = builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddIdentity<Register, IdentityRole>()
        .AddEntityFrameworkStores<Applicationdbcontext>()
        .AddDefaultTokenProviders();
builder.Services.AddScoped<UserManager<Register>>();   
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>());

//.AddFluentValidation(options =>
//{
//   // Validate child properties and root collection elements
//   options.ImplicitlyValidateChildProperties = true;
//   options.ImplicitlyValidateRootCollectionElements = true;

//   // Automatic registration of validators in assembly
//   options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
//} );


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<RegisterDTO>,RegisterValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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
app.UseCors("MyCors");
app.MapControllers();
app.Run();

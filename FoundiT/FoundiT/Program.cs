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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

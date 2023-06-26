
using Application.IRepository.Repository;
using Application.IRepository;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Hosting;
using Infrastructure.DtoMapping;
using Azure.Storage.Blobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string cs = builder.Configuration.GetConnectionString("cnStr");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(cs));
builder.Services.AddScoped<IRegisterRepository, RegisterRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IImageStorageService>(provider =>
    {
        var connectionString = builder.Configuration["AzureBlobStorage:ConnectionString"];
        var containerName = builder.Configuration["AzureBlobStorage:ContainerName"];
        var blobServiceClient = new BlobServiceClient(connectionString);
        return new AzureBlobStorageService(blobServiceClient, containerName);
    });



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

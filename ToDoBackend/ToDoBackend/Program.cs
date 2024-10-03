
using System.Security.Cryptography;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ToDoBackend;
using ToDoBackend.API.Validators;
using ToDoBackend.API.Validators.Group;
using ToDoBackend.API.Validators.ToDoItem;
using ToDoBackend.DataContext;
using ToDoBackend.Domain;
using ToDoBackend.Domain.Helpers;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;
using ToDoBackend.DTO.ToDoItem;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDataContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetValue<string>("DB_CONNECTION"))
    );

builder.Configuration.AddEnvironmentVariables();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOptions<ToDoServiceOptions>().Bind(builder.Configuration)
    .ValidateDataAnnotations()
    .ValidateOnStart();

//Endpoint Services

builder.Services.AddScoped<IToDoService, ToDoService>();

builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IAuthHelper, AuthHelper>(); //can prob be singleton

//Model Validators
builder.Services
    .AddScoped<IValidator<CreateGroupRequest>, CreateGroupRequestValidator>()
    .AddScoped<IValidator<UpdateGroupRequest>, UpdateGroupRequestValidator>()
    .AddScoped<IValidator<CreateToDoItemRequest>, CreateToDoItemRequestValidator>()
    .AddScoped<IValidator<UpdateToDoItemRequest>, UpdateToDoItemRequestValidator>()
    .AddScoped<IValidator<PatchToDoItemRequest>, PatchToDoItemRequestValidator>();

//Authentication

var rsa = RSA.Create();
var privateKeyBase64 = builder.Configuration.GetValue<string>("RSA_PRIVATE")??"";
var privateKeyBytes = Convert.FromBase64String(privateKeyBase64);
rsa.ImportRSAPrivateKey(privateKeyBytes,out _);
var rsaSecurityKey = new RsaSecurityKey(rsa);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "iss",
            ValidAudience = "aud",
            IssuerSigningKey = rsaSecurityKey
        };
    });

builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


//CORS

builder.Services.AddCors(setupAction => setupAction.AddPolicy("lenientPolicy", corsPolicyBuilder =>
{
    corsPolicyBuilder.AllowAnyMethod();
    corsPolicyBuilder.AllowAnyOrigin();
    corsPolicyBuilder.AllowAnyHeader();
}));


//fix swagger auth setup
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer {token}`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddControllers();

builder.Services.AddHealthChecks();



var app = builder.Build();


app.UseCors("lenientPolicy");


app.MapGet("/", () => "Welcome to ToDo!");

app.MapGroup("")
    .MapToDoEndpoints()
    .MapGroupEndpoints()
    .MapUserEndpoints()
    .MapControllers();

    app.UseSwagger();
    app.UseSwaggerUI();



using (var scope = app.Services.CreateScope())
{
    var dbContext =  scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
    dbContext.Database.Migrate();
}

app.Run();



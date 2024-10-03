
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ToDoBackend;
using ToDoBackend.API.Validators.Group;
using ToDoBackend.API.Validators.ToDoItem;
using ToDoBackend.DataContext;
using ToDoBackend.Domain;
using ToDoBackend.Domain.Helpers;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;
using ToDoBackend.DTO.ToDoItem;
using ToDoBackend.DTO.User;

//var builder = WebApplication.CreateBuilder(args);

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
    {
      options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    });

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
if (builder.Environment.IsDevelopment())
{


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
}

builder.Services.AddHealthChecks();



var app = builder.Build();


app.UseCors("lenientPolicy");


app.MapGet("/", () => "Welcome to ToDo!");

app.MapGroup("")
    .MapToDoEndpoints()
    .MapGroupEndpoints()
    .MapUserEndpoints();

    app.UseSwagger();
    app.UseSwaggerUI();


    if (app.Environment.IsDevelopment())
    {
    using (var scope = app.Services.CreateScope())
{
    var dbContext =  scope.ServiceProvider.GetRequiredService<ApplicationDataContext>();
    dbContext.Database.Migrate();
}
    }
app.Run();


[JsonSerializable(typeof(CreateToDoItemRequest))]
[JsonSerializable(typeof(PatchToDoItemRequest))]
[JsonSerializable(typeof(ToDoItemResponse))]
[JsonSerializable(typeof(UpdateToDoItemRequest))]
[JsonSerializable(typeof(CreateGroupRequest))]
[JsonSerializable(typeof(GroupResponse))]
[JsonSerializable(typeof(UpdateGroupRequest))]
[JsonSerializable(typeof(CreateAccountRequest))]
[JsonSerializable(typeof(DeleteAccountRequest))]
[JsonSerializable(typeof(LoginRequest))]
[JsonSerializable(typeof(UserResponse))]
[JsonSerializable(typeof(DeletionRequest))]
[JsonSerializable(typeof(DeletionResponse))]
    internal partial class AppJsonSerializerContext : JsonSerializerContext
{ }
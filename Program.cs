using LearnJWTAuthentication.Middlewares;
using LearnJWTAuthentication.Models;
using LearnJWTAuthentication.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Appsettings"));
builder.Services.AddScoped<IUserServices, UserService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation    
                swagger.SwaggerDoc("v1", new OpenApiInfo  
                {  
                    Version = "v1",  
                    Title = "ASP.NET 5 Web API",  
                    Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"  
                });  
                // To Enable authorization using Swagger (JWT)    
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()  
                {  
                    Name = "Authorization",  
                    Type = SecuritySchemeType.ApiKey,  
                    Scheme = "Bearer",  
                    BearerFormat = "JWT",  
                    In = ParameterLocation.Header,  
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",  
                });  
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement  
                {  
                    {  
                          new OpenApiSecurityScheme  
                            {  
                                Reference = new OpenApiReference  
                                {  
                                    Type = ReferenceType.SecurityScheme,  
                                    Id = "Bearer"  
                                }  
                            },  
                            new string[] {}  
  
                    }  
                });  
});

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET 5 Web API v1")); 
}

app.UseHttpsRedirection();  
  
app.UseRouting(); 

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseMiddleware<JWTMiddleware>();

app.UseAuthentication();  
app.UseAuthorization();  

app.UseEndpoints(endpoints =>  
{  
    endpoints.MapControllers();  
}); 

app.Run();

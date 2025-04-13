using eStore.DAL.Repositories;
using AutoMapper;
using eStore.BL.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace eStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Swagger with JWT support
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "eStore API", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Enter only the JWT Bearer token (no 'Bearer' prefix)",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            // Enable CORS (Allow All)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });

            // Add Auth0 Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://testmyfirstapp.us.auth0.com/";
                options.Audience = "https://api.local.dev";

             options.TokenValidationParameters = new TokenValidationParameters
{
    NameClaimType = "name",
    RoleClaimType = "roles"
};

                options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("AUTH FAILED: " + context.Exception.Message);
            return Task.CompletedTask;
        }
    };

            });

            // Antiforgery (optional for APIs)
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.Name = "XSRF-TOKEN";
                options.Cookie.HttpOnly = false;
            });

            // Repositories
            builder.Services.AddScoped<ICustomerRepository>(sp =>
                new CustomerRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<ICategoryRepository>(sp =>
                new CategoryRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<IProductRepository>(sp =>
                new ProductRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<IOrderDetailRepository>(sp =>
                new OrderDetailRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<IOrderRepository>(sp =>
                 new OrderRepository(
                      sp.GetRequiredService<IConfiguration>(),
                      "DefaultConnection",
                      sp.GetRequiredService<IOrderDetailRepository>()
                  ));

            // Services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(eStoreAPI.Common.MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            // Optional antiforgery token generator for frontend integration
            app.Use(async (context, next) =>
            {
                var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();

                if (context.Request.Method == "GET")
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                }
                await next(context);
            });

            app.MapControllers();

            app.Run();
        }
    }
}

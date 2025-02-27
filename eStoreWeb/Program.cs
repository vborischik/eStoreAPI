
using eStore.DAL.Repositories;     
using AutoMapper;
using eStore.BL.Services;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Antiforgery;



namespace eStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
          
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF_TOKEN";
                options.Cookie.Name = "XSRF-TOKEN";
                options.Cookie.HttpOnly = false;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
                }
                
                );

            // Register the CustomerRepository with the DI container.
            // Pass IConfiguration and connection string name ("DefaultConnection") to the constructor.
            builder.Services.AddScoped<ICustomerRepository>(sp =>
                new CustomerRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<ICategoryRepository>(sp =>
               new CategoryRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<IProductRepository>(sp =>
              new ProductRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));


            // Register the CustomerService (Business Logic Layer)
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Register AutoMapper with the MappingProfile from eStore.Common.Mappings.
            builder.Services.AddAutoMapper(typeof(eStoreAPI.Common.MappingProfile));
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors(options => { options.AllowAnyOrigin();options.AllowAnyHeader();options.AllowAnyMethod(); });

            app.Use(async (context, next) =>
            {
                var antiforgery = context.RequestServices.GetRequiredService<IAntiforgery>();
              
                if (context.Request.Method == "GET")
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                }
                await next(context);
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}

using eStore.DAL.Repositories;
using AutoMapper;
using eStore.BL.Services;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Options;

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
            builder.Services.AddSwaggerGen();

            // Enable CORS (Allow All)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Антифрод-защита (если нужна)
            builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.Cookie.Name = "XSRF-TOKEN";
                options.Cookie.HttpOnly = false;
            });

            // Регистрация репозиториев
            builder.Services.AddScoped<ICustomerRepository>(sp =>
                new CustomerRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<ICategoryRepository>(sp =>
                new CategoryRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            builder.Services.AddScoped<IProductRepository>(sp =>
                new ProductRepository(sp.GetRequiredService<IConfiguration>(), "DefaultConnection"));

            // Регистрация сервисов
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Регистрация AutoMapper
            builder.Services.AddAutoMapper(typeof(eStoreAPI.Common.MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            
            app.UseRouting(); // 👈 Добавил правильный порядок
            
            app.UseCors("AllowAll"); // 👈 Вызван перед UseAuthorization

            app.UseAuthorization();
            
            // Если нужен Antiforgery, можно оставить этот код
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

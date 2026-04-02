using Microsoft.EntityFrameworkCore;
using Product.Interfaces;
using Product.Models;
using Product.Services;

namespace Product
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .SetIsOriginAllowed(origin =>
                        {
                            if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                                return false;

                            return uri.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase);
                        })
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            // Add DbContext
            var connectionString = builder.Configuration.GetConnectionString("ProductConnection");
            builder.Services.AddDbContext<ProductContext>(options =>
                options.UseSqlServer(connectionString));

            // Register service
            builder.Services.AddScoped<IProduct, ProductService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("FrontendPolicy");

            app.UseAuthorization();

            app.MapGet("/", () => Results.Ok(new
            {
                message = "Product API is running",
                swagger = "/swagger",
                products = "/api/Product"
            }));

            app.MapControllers();

            app.Run();
        }
    }
}

using Azure.Storage.Blobs;
using AzureBlobProject.Services;

namespace AzureBlobProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSingleton(u => new BlobServiceClient(builder.Configuration.GetValue<string>("BlobConnection")));
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton<IContainerService, ContainerService>();// add this above code is already there 

            builder.Services.AddSingleton<IContainerService, ContainerService>();
            builder.Services.AddSingleton<IBlobService, BlobService>();// this line add it not above one it is for your understanding where to write okay 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

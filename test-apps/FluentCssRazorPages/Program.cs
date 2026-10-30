using FluentCss.Middleware;

namespace FluentCssRazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddFluentCss();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseRouting();

            // Serves /fluentcss/hash and /fluentcss/{hash}.css.
            app.UseFluentCss();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
                .WithStaticAssets();

            app.Run();
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlayerService.Database;
using PlayerService.MessageHandlers;
using Shared;

namespace PlayerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSharedServices("Player Service");
            services.AddDbContext<PlayerDbContext>();

            services.AddMessagePublishing("QuestService", builder => {
                // This tells the application that when it receives a `QuestCompleted` message, it should handle it using the `QuestCompletedMessageHandler`.
                builder.WithHandler<QuestCompletedMessageHandler>("QuestCompleted");
            });

            using var db = new PlayerDbContext();
            db.Database.EnsureCreated();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSharedAppParts("Player Service");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

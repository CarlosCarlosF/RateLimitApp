using Funq;
using Infrastracture;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RateLimitApplication.Services;
using ServiceStack;
using ServiceStack.Configuration;
using ServiceStack.Redis;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace RateLimitApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                         .SetBasePath(env.ContentRootPath)
                         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = new AppSettings();

            services.AddSingleton<IRedisClientsManagerTransient>(provider =>
            {
                var connection = Configuration.GetConnectionString("RedisConnection");
                PooledRedisClientManager transientRedisPoolManager = new PooledRedisClientManager(new[] { connection }, new[] { connection }, null, 1, 100, 10);
                return new PooledRedisClientManagerTransient(transientRedisPoolManager);
            });
            ContainerRegistrations.Register(services, appSettings);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseServiceStack(new AppHost());
        }
        public class AppHost : AppHostBase
        {
            public AppHost() : base("RateLimitApplication", typeof(RateLimitService).Assembly) { }

            public override void Configure(Container container)
            {
                base.SetConfig(new HostConfig
                {
                    DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), false)
                });
            }
        }
    }
}

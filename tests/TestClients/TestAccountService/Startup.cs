using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using TaskUtil = MassTransit.Util.TaskUtil;

namespace TestAccountService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = configurationBuilder.Build();
        }

		public IContainer DIContainer { get; private set; }

        public IConfigurationRoot Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
			services.AddMvc();
	        services.AddSwaggerGen(c =>
	        {
		        c.SwaggerDoc("v1", new Info {Title = "TestAccount API", Version = "v1"});
	        });

	        var containerBuilder = new ContainerBuilder();
	        containerBuilder.Register(c =>
		        {
			        return Bus.Factory.CreateUsingRabbitMq(sbc =>
				        sbc.Host(new Uri("rabbitmq://localhost"), host =>
				        {
					        host.Username("guest");
					        host.Password("guest");
				        })
			        );
		        })
		        .As<IBusControl>()
		        .As<IBus>()
		        .As<IPublishEndpoint>()
		        .SingleInstance();

			containerBuilder.Populate(services);
	        DIContainer = containerBuilder.Build();

	        return new AutofacServiceProvider(DIContainer);
        }

        public void Configure(IApplicationBuilder app, 
			IHostingEnvironment env, 
			ILoggerFactory loggerFactory,
			IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

	        app.UseSwagger();

	        app.UseSwaggerUI(c =>
	        {
		        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestAccount API V1");
	        });

	        var bus = DIContainer.Resolve<IBusControl>();
	        var busHandle = TaskUtil.Await(() => bus.StartAsync());

			appLifetime.ApplicationStopping.Register(() => busHandle.Stop());

            app.UseMvc();
        }
    }
}

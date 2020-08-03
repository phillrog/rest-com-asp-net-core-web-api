using AutoMapper;
using DevIO.Api.Configuration;
using DevIO.Api.Configurations;
using DevIO.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using DevIO.Api.Extensions;

namespace DevIO.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }

		public Startup(IHostingEnvironment hostEnvironment)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(hostEnvironment.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
				.AddEnvironmentVariables();

			if (hostEnvironment.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<MeuDbContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
			);

			services.AddAutoMapper(typeof(Startup));

			services.AddApiConfig();

			services.AddIdentityConfig(Configuration);

			services.AddSwaggerConfig();

			services.AddLoggingConfig(Configuration);

			services.ResolveDepencies();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			app.UseApiConfig(env);

			app.UseAuthentication();

			app.UseMiddleware<ExceptionMiddleware>();

			app.UseSwaggerConfig(provider);

			app.UseLoggingConfiguration();			
		}
	}
}

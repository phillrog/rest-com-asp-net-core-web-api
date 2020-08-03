﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;

namespace DevIO.Api.Configuration
{
	public static class ApiConfig
	{
		public static IServiceCollection AddApiConfig(this IServiceCollection services)
		{
			services.AddControllers();

			services.AddApiVersioning(options =>
			{
				options.AssumeDefaultVersionWhenUnspecified = true;
				options.DefaultApiVersion = new ApiVersion(1, 0);
				options.ReportApiVersions = true;
			});

			services.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;

			});

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			services.AddCors(options =>
			{
				options.AddPolicy("Development",
					builder =>
						builder
							.AllowAnyOrigin()
							.AllowAnyMethod()
							.AllowAnyHeader()
							.AllowCredentials());

				options.AddPolicy("Production",
					builder =>
						builder
							.WithMethods("GET")
							.WithOrigins("http://desenvolvedor.io")
							.SetIsOriginAllowedToAllowWildcardSubdomains()
							//.WithHeaders(HeaderNames.ContentType, "x-custom-header")
							.AllowAnyHeader());
			});

			return services;
		}

		public static IApplicationBuilder UseApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseCors("Development");
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseCors("Production");
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			return app;
		}
	}
}
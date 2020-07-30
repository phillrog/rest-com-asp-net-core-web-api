using DevIO.Business.Interfaces;
using DevIO.Data.Contexts;
using DevIO.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.Configurations
{
	public static class DependencyInjectionConfig
	{
		public static IServiceCollection ResolveDepencies(this IServiceCollection services)
		{
			services.AddScoped<MeuDbContext>();
			services.AddScoped<IFornecedorRepository, FornecedorRepository>();

			return services;
		}
	}
}

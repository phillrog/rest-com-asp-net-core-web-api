using DevIO.Api.Extensions;
using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;
using DevIO.Business.Services;
using DevIO.Data.Contexts;
using DevIO.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DevIO.Api.Configurations
{
	public static class DependencyInjectionConfig
	{
		public static IServiceCollection ResolveDepencies(this IServiceCollection services)
		{
			services.AddScoped<MeuDbContext>();
			services.AddScoped<IFornecedorRepository, FornecedorRepository>();
			services.AddScoped<IEnderecoRepository, EnderecoRepository>();
			services.AddScoped<IProdutoRepository, ProdutoRepository>();

			services.AddScoped<INotificador, Notificador>();
			services.AddScoped<IFornecedorService, FornecedorService>();
			services.AddScoped<IProdutoService, ProdutoService>();

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IUser, AspNetUser>();

			return services;
		}
	}
}

using System;
using System.Linq;
using DevIO.Business.Interfaces;
using DevIO.Business.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DevIO.Api.Controllers
{
	[ApiController]
	public class MainController : ControllerBase
	{
		private readonly INotificador _notificador;
		private readonly IUser _appUser;

		public Guid UsuarioId { get; }
		public bool UsuarioAutenticado { get; }

		public MainController(INotificador notificador,
			IUser appUser)
		{
			_notificador = notificador;
			_appUser = appUser;

			if (appUser.IsAuthenticated())
			{
				UsuarioId = appUser.GetUserId();
				UsuarioAutenticado = true;
			}
		}

		protected bool OperacaoValida()
		{
			return !_notificador.TemNotifcacao();
		}

		protected ActionResult CustomResponse(object result = null)
		{
			if (OperacaoValida())
			{
				return Ok(new
				{
					success = true,
					data = result
				});
			}

			return BadRequest(new
			{
				success = false,
				errors = _notificador.ObterNotificacoes().Select(n => n.Mensagem)
			});
		}

		protected ActionResult CustomResponse(ModelStateDictionary modelState)
		{
			if (!modelState.IsValid) NotificarErroModelInvalida(modelState);

			return CustomResponse();
		}

		protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
		{
			var erros = modelState.Values.SelectMany(e => e.Errors);

			foreach (var erro in erros)
			{
				var errorMsg = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;

				NotificarErro(errorMsg);
			}
		}

		protected void NotificarErro(string mensagem)
		{
			_notificador.Handle(new Notificacao(mensagem));
		}
	}
}

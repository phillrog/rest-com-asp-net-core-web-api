using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
	public class ProdutosController : MainController
	{
		private readonly IMapper _mapper;
		private readonly IProdutoRepository _produtoRepository;
		private readonly IProdutoService _produtoService;

		public ProdutosController(INotificador notificador,
			IProdutoRepository produtoRepository,
			IProdutoService produtoService,
			IMapper mapper) : base(notificador)
		{
			_mapper = mapper;
			_produtoRepository = produtoRepository;
			_produtoService = produtoService;
		}

		[HttpGet]
		public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
		{
			return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosPorFornecedores());
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
		{
			var produtosViewModel = await ObterProduto(id);

			if (produtosViewModel == null) return NotFound();

			return produtosViewModel;
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
		{
			var produtosViewModel = await ObterProduto(id);

			if (produtosViewModel == null) return NotFound();

			await _produtoService.Remover(id);

			return CustomResponse(produtosViewModel);
		}

		public async Task<ProdutoViewModel> ObterProduto(Guid id)
		{
			return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutosPorFornecedor(id));
		}
	}
}

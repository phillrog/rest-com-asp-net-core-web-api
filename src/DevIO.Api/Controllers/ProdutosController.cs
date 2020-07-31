using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
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

		[HttpPost]
		public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
		{
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;
			if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
			{
				return CustomResponse(produtoViewModel);
			}

			produtoViewModel.Imagem = imagemNome;
			await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

			return CustomResponse(produtoViewModel);
		}

		[HttpDelete("{id:guid}")]
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

		private bool UploadArquivo(string arquivo, string imgNome)
		{
			if (string.IsNullOrEmpty(arquivo))
			{
				NotificarErro("Forneça uma imagem para este produto!");
				return false;
			}

			var imageDataByteArray = Convert.FromBase64String(arquivo);

			var filePath = Path.Combine(Directory.GetCurrentDirectory(), "../DevIO.Portal/src/assets", imgNome);

			if (System.IO.File.Exists(filePath))
			{
				NotificarErro("Já existe um arquivo com este nome!");
				return false;
			}

			System.IO.File.WriteAllBytes(filePath, imageDataByteArray);

			return true;
		}
	}
}

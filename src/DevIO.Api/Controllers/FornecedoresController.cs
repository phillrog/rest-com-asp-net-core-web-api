using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
	[Route("api/[controller]")]
	public class FornecedoresController : MainController
	{
		private readonly IFornecedorRepository _fornecedorRepository;
		private readonly IMapper _mapper;
		private readonly IFornecedorService _fornecedorService;

		public FornecedoresController(IFornecedorRepository fornecedorRepository,
			IMapper mapper,
			IFornecedorService fornecedorService)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
			_fornecedorService = fornecedorService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
		{
			var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

			return Ok(fornecedores);
		}

		[HttpGet("{id:guid}")]
		public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterPorId(Guid id)
		{
			var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));

			if (fornecedor == null) return NotFound();

			return Ok(fornecedor);
		}

		[HttpPost]
		public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
		{
			if (!ModelState.IsValid) return BadRequest();

			var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
			var result = await _fornecedorService.Adicionar(fornecedor);

			if (!result) return BadRequest();

			return Ok(fornecedor);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
		{
			if (id != fornecedorViewModel.Id) return BadRequest();

			if (!ModelState.IsValid) return BadRequest();

			var fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
			var result = await _fornecedorService.Atualizar(fornecedor);

			if (!result) return BadRequest();

			return Ok(fornecedor);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
		{
			var fornecedor = ObterFornecedorEndereco(id);

			if (fornecedor == null) return NotFound();

			var result = await _fornecedorService.Remover(id);

			if (!result) return BadRequest();

			return Ok(fornecedor);
		}

		public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
		{
			return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
		}

		public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
		{
			return _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
		}
	}
}
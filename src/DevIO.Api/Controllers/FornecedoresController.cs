﻿using System;
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
		private readonly IEnderecoRepository _enderecoRepository;

		public FornecedoresController(IFornecedorRepository fornecedorRepository,
			IMapper mapper,
			IFornecedorService fornecedorService,
			INotificador notificador,
			IEnderecoRepository enderecoRepository) : base(notificador)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
			_fornecedorService = fornecedorService;
			_enderecoRepository = enderecoRepository;
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
			if (!ModelState.IsValid) return CustomResponse(ModelState);

			await _fornecedorService.Adicionar(_mapper.Map<Fornecedor>(fornecedorViewModel));

			return CustomResponse(fornecedorViewModel);
		}

		[HttpPut("{id:guid}")]
		public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
		{
			if (id != fornecedorViewModel.Id)
			{
				NotificarErro("O id informado não é o mesmo que foi passado na query");

				return CustomResponse(fornecedorViewModel);
			}

			if (!ModelState.IsValid) return CustomResponse(ModelState);

			await _fornecedorService.Atualizar(_mapper.Map<Fornecedor>(fornecedorViewModel));

			return CustomResponse(fornecedorViewModel);
		}

		[HttpDelete("{id:guid}")]
		public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
		{
			var fornecedorViewModel = ObterFornecedorEndereco(id);

			if (fornecedorViewModel == null) return NotFound();

			await _fornecedorService.Remover(id);

			return CustomResponse(fornecedorViewModel);
		}

		[HttpPut("atualiza-endereco/{id:guid}")]
		public async Task<IActionResult> AtualizaEndereco(Guid id, EnderecoViewModel enderecoViewModel)
		{
			if (id != enderecoViewModel.Id)
			{
				NotificarErro("O id informado não é o mesmo que foi passado na query");

				return CustomResponse(enderecoViewModel);
			}

			if (!ModelState.IsValid) return CustomResponse(ModelState);

			await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));

			return CustomResponse(enderecoViewModel);
		}

		[HttpGet("obter-endereco/{id:guid}")]
		public async Task<EnderecoViewModel> ObeterEnderecoPorId(Guid id)
		{
			return _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterPorId(id));
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
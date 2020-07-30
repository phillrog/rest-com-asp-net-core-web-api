﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.Api.Controllers
{
	[Route("api/[controller]")]
	public class FornecedoresController : MainController
	{
		private readonly IFornecedorRepository _fornecedorRepository;
		private readonly IMapper _mapper;

		public FornecedoresController(IFornecedorRepository fornecedorRepository,
			IMapper mapper)
		{
			_fornecedorRepository = fornecedorRepository;
			_mapper = mapper;
		}

		public async Task<ActionResult<IEnumerable<FornecedorViewModel>>> ObterTodos()
		{
			var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

			return Ok(fornecedores);
		}
	}
}
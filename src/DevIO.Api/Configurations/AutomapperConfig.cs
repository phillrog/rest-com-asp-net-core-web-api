﻿using DevIO.Api.ViewModels;
using DevIO.Business.Models;
using AutoMapper;

namespace DevIO.Api.Configurations
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
			CreateMap<Produto, ProdutoViewModel>().ReverseMap();
			CreateMap<Endereco, EnderecoViewModel>().ReverseMap();
		}
	}
}

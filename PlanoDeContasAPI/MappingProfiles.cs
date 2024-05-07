using Application.Dtos;
using AutoMapper;
using Domain.Models;
using System.Net;

namespace PlanoDeContasAPI
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ContaDto, Conta>();
            CreateMap<Conta, ContaDto>();
            CreateMap<Conta, ContaListaDto>();
            CreateMap<ContaListaDto, Conta>();
        }
    }
}

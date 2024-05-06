using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlanoDeContasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContaController : ControllerBase
    {
        private readonly IContaService _contaService;
        private readonly IMapper mapper;

        public ContaController(IContaService contaService, IMapper mapper)
        {
            _contaService = contaService;
            this.mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _contaService.RecuperarPorIdAsync(id);
                return Ok(mapper.Map<ContaDto>(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetById(string codigo)
        {
            try
            {
                return Ok(await _contaService.RecuperarPorCodigoAsync(codigo));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContaDto conta)
        {
            try
            {
                var contaDominio = mapper.Map<Conta>(conta);
                var result = await _contaService.CriarContaAsync(contaDominio);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

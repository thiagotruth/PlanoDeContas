using Application.Dtos;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _contaService.RecuperarTodasAsync();
                return Ok(mapper.Map<IEnumerable<ContaListaDto>>(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> FilterByName([FromQuery] string name)
        {
            try
            {
                var result = await _contaService.FiltrarPorNomeAsync(name);
                return Ok(mapper.Map<IEnumerable<ContaListaDto>>(result));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
        public async Task<IActionResult> GetByCode(string codigo)
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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _contaService.ExcluirContaAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/proximocodigo")]
        public async Task<IActionResult> SugerirProximoCodigo(int id)
        {
            try
            {
                return Ok(await _contaService.SugerirProximoCodigoAsync(id));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

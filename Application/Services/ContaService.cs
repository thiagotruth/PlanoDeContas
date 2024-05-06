using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _contaRepository;

        public ContaService(IContaRepository contaRepository)
        {
            _contaRepository = contaRepository;
        }
        public async Task<Conta> CriarContaAsync(Conta conta)
        {
            await ValidarContaPai(conta);

            await ValidarCodigo(conta);

           return await _contaRepository.CriarContaAsync(conta);

        }
        public async Task ExcluirContaAsync(int idConta)
        {
            throw new NotImplementedException();
        }

        public Task<Conta?> RecuperarPorCodigoAsync(string codigo)
        {
            return _contaRepository.RecuperarPorCodigoAsync(codigo);
        }

        public Task<Conta?> RecuperarPorIdAsync(int id)
        {
            return _contaRepository.RecuperarPorIdAsync(id);
        }

        private async Task ValidarCodigo(Conta conta)
        {
            if (!Regex.IsMatch(conta.Codigo, @"^\d+(\.\d+)*$"))
                throw new ArgumentException("Erro: Código inválido. O código deve conter somente números e pontos.");

            string finalCodigo = conta.Codigo.Split('.').Last();

            if (finalCodigo.Length > 4)
                throw new ArgumentException("O código excede 9999.");

            Conta? contaExistente = await _contaRepository.RecuperarPorCodigoAsync(conta.Codigo);

            if (contaExistente is not null)
                throw new ArgumentException("Erro: Código existente.");
        }

        private async Task ValidarContaPai(Conta conta)
        {
            Conta? contaPai = null;

            if (conta.IdContaPai is not null)
                contaPai = await _contaRepository.RecuperarPorIdAsync((int)conta.IdContaPai);

            if (contaPai is not null && contaPai.AceitaLancamento)
                throw new ArgumentException("Erro: Esta conta não aceita filhos pois é uma conta que aceita lançamentos.");

            if (contaPai is not null && contaPai.TipoConta != conta.TipoConta)
                throw new ArgumentException("Erro: A conta a ser inserida não é do mesmo tipo da conta pai.");
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task CriarContaAsync(Conta conta)
        {
            await ValidarContaPai(conta);

            await ValidarCodigo(conta);
        }
        public async Task ExcluirContaAsync(int idConta)
        {
            throw new NotImplementedException();
        }

        private async Task ValidarCodigo(Conta conta)
        {
            string finalCodigo = conta.Codigo.Split('.').Last();

            if (finalCodigo.Length > 4)
                throw new Exception();

            Conta? contaExistente = await _contaRepository.RecuperarPorCodigoAsync(conta.Codigo);

            if (contaExistente is not null)
                throw new Exception();
        }

        private async Task ValidarContaPai(Conta conta)
        {
            Conta? contaPai = null;

            if (conta.IdPai is not null)
                contaPai = await _contaRepository.RecuperarPorIdAsync((int)conta.IdPai);

            if (contaPai is not null && (contaPai.AceitaLancamento || contaPai.TipoConta != conta.TipoConta))
                throw new Exception();
        }

    }
}

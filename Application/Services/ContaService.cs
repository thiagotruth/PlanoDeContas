using Domain.Interfaces;
using Domain.Models;
using System.Text.RegularExpressions;

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
            await _contaRepository.ExcluirContaAsync(idConta);
        }

        public async Task<Conta?> RecuperarPorCodigoAsync(string codigo)
        {
            return await _contaRepository.RecuperarPorCodigoAsync(codigo);
        }

        public async Task<Conta?> RecuperarPorIdAsync(int id)
        {
            return await _contaRepository.RecuperarPorIdAsync(id);
        }

        public async Task<string> SugerirProximoCodigoAsync(int contaId)
        {
            var conta = await _contaRepository.RecuperarPorIdAsync(contaId) ?? throw new ArgumentException("Conta inexistente.");

            if (conta.ContasFilhas?.Count == 0)
            {
                return $"{conta.Codigo}.1";
            }

            var maiorCodigoFilha = conta.ContasFilhas?
                .Select(c => int.Parse(c.Codigo.Split('.').Last()))
                .DefaultIfEmpty(0)
                .Max();

            if (maiorCodigoFilha < 999)
            {
                return $"{conta.Codigo}.{maiorCodigoFilha + 1}";
            }

            var proximoCodigo = CalcularProximoCodigoHierarquia($"{conta.Codigo}.{maiorCodigoFilha}");

            if (await _contaRepository.CodigoExisteAsync(proximoCodigo))
            {
                var codigosExistentes = await _contaRepository.RecuperarTodosOsCodigosAsync();

                while (codigosExistentes.Contains(proximoCodigo))
                {
                    proximoCodigo = CalcularProximoCodigoHierarquia(proximoCodigo);
                }
            }

            return proximoCodigo;
        }

        private static string CalcularProximoCodigoHierarquia(string codigo)
        {
            List<string> codigoPartes = codigo.Split('.').ToList();

            for (int i = codigoPartes.Count - 1; i >= 0; i--)
            {
                if (int.TryParse(codigoPartes[i], out int ultimaParte))
                {
                    if (ultimaParte < 999)
                    {
                        codigoPartes[i] = (ultimaParte + 1).ToString();
                        return string.Join(".", codigoPartes);
                    }
                    else
                    {
                        codigoPartes.RemoveAt(i);
                    }
                }
            }
            throw new ArgumentException("Número de contas possíveis excedido");
        }

        private async Task ValidarCodigo(Conta conta)
        {
            if (!Regex.IsMatch(conta.Codigo, @"^\d+(\.\d+)*$"))
                throw new ArgumentException("Erro: Código inválido. O código deve conter somente números e pontos.");

            string finalCodigo = conta.Codigo.Split('.').Last();

            if (finalCodigo.Length > 3)
                throw new ArgumentException("O código excede 999.");

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

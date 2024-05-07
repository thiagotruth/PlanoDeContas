using Domain.Models;

namespace Domain.Interfaces
{
    public interface IContaService
    {
        Task<IEnumerable<Conta>> RecuperarTodasAsync();
        Task<Conta> CriarContaAsync(Conta conta);
        Task<Conta?> RecuperarPorIdAsync(int id);
        Task<Conta?> RecuperarPorCodigoAsync(string codigo);
        Task ExcluirContaAsync(int idConta);
        Task<string> SugerirProximoCodigoAsync(int contaId);
        Task<IEnumerable<Conta>> FiltrarPorNomeAsync(string nome);
    }
}

using Domain.Models;

namespace Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<Conta?> RecuperarPorIdAsync(int id);
        Task<Conta?> RecuperarPorCodigoAsync(string codigo);
        Task<Conta> CriarContaAsync(Conta conta);
        Task ExcluirContaAsync(int idConta);
    }
}

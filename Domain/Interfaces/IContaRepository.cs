using Domain.Models;

namespace Domain.Interfaces
{
    public interface IContaRepository
    {
        Task<IEnumerable<Conta>> RecuperarTodasAsync();
        Task<IEnumerable<Conta>> FiltrarPorNomeAsync(string nome);
        Task<Conta?> RecuperarPorIdAsync(int id);
        Task<Conta?> RecuperarPorCodigoAsync(string codigo);
        Task<Conta> CriarContaAsync(Conta conta);
        Task ExcluirContaAsync(int idConta);
        Task<IEnumerable<string>> RecuperarTodosOsCodigosAsync();
        Task<bool> CodigoExisteAsync(string codigo);        
    }
}

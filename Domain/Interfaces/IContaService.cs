using Domain.Models;

namespace Domain.Interfaces
{
    public interface IContaService
    {
        Task CriarContaAsync(Conta conta);
        Task ExcluirContaAsync(int idConta);
    }
}

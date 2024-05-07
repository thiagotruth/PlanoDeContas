using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly PlanoDeContasContext context;

        public ContaRepository(PlanoDeContasContext context)
        {
            this.context = context;
        }

        public async Task<bool> CodigoExisteAsync(string codigo) =>
            await context.Contas.AnyAsync(c => c.Codigo == codigo);

        public async Task<Conta> CriarContaAsync(Conta conta)
        {
            await context.Contas.AddAsync(conta);
            await context.SaveChangesAsync();
            return conta;
        }

        public async Task ExcluirContaAsync(int idConta) =>
            await context.Contas.Where(c => c.Id == idConta).ExecuteDeleteAsync();


        public async Task<Conta?> RecuperarPorCodigoAsync(string codigo) =>
            await context.Contas.Include(c => c.ContasFilhas).FirstOrDefaultAsync(c => c.Codigo == codigo);


        public async Task<Conta?> RecuperarPorIdAsync(int id) =>
            await context.Contas.Include(c => c.ContasFilhas).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<IEnumerable<Conta>> RecuperarTodasAsync() =>
            await context.Contas.IgnoreAutoIncludes().OrderBy(c => c.Codigo).ToListAsync();


        public async Task<IEnumerable<string>> RecuperarTodosOsCodigosAsync() =>
            await context.Contas.Select(c => c.Codigo).ToListAsync();

        public async Task<IEnumerable<Conta>> FiltrarPorNomeAsync(string nome) =>
            await context.Contas.IgnoreAutoIncludes().Where(c => c.Nome.Contains(nome)).ToListAsync();
    }
}

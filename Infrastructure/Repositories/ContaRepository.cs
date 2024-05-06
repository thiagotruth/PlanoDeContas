using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly PlanoDeContasContext context;

        public ContaRepository(PlanoDeContasContext context)
        {
            this.context = context;
        }

        public async Task<Conta> CriarContaAsync(Conta conta)
        {
            await context.Contas.AddAsync(conta);            
            await context.SaveChangesAsync();
            return conta;
        }

        public async Task ExcluirContaAsync(int idConta) =>
            await context.Contas.Where(c => c.Id == idConta).ExecuteDeleteAsync();


        public async Task<Conta?> RecuperarPorCodigoAsync(string codigo) =>
            await context.Contas.FirstOrDefaultAsync(c => c.Codigo == codigo);


        public async Task<Conta?> RecuperarPorIdAsync(int id) =>
            await context.Contas.Include(c => c.ContasFilhas).FirstOrDefaultAsync(c => c.Id == id);

    }
}

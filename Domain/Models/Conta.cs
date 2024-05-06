namespace Domain.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public int? IdPai { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoConta TipoConta { get; set; }
        public bool AceitaLancamento { get; set; }
        public IEnumerable<Conta>? ContasFilhas { get; set; }
    }

    public enum TipoConta
    {
        Despesa = 1,
        Receita = 2,
    }
}

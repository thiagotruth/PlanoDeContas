namespace Domain.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public int? IdContaPai { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public TipoConta TipoConta { get; set; }
        public bool AceitaLancamento { get; set; }
        public List<Conta>? ContasFilhas { get; set; }
    }

    public enum TipoConta
    {
        Despesa = 1,
        Receita = 2,
    }
}

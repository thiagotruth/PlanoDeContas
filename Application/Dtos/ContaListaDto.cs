using Domain.Models;

namespace Application.Dtos
{
    public class ContaListaDto
    {
        public int Id { get; set; }
        public int? IdContaPai { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoConta TipoConta { get; set; }
        public bool AceitaLancamento { get; set; }
    }
}

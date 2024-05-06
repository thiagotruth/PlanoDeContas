using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class ContaDto
    {
        public int Id { get; set; }
        public int? IdContaPai { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; } = string.Empty;
        public TipoConta TipoConta { get; set; }
        public bool AceitaLancamento { get; set; }
        public List<ContaDto>? ContasFilhas { get; set; }
    }
}

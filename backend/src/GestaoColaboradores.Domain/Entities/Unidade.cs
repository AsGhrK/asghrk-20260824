using GestaoColaboradores.Domain.Common;

namespace GestaoColaboradores.Domain.Entities;

public class Unidade : CadastroBase
{
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public ICollection<Colaborador> Colaboradores { get; set; } = new List<Colaborador>();
}

using GestaoColaboradores.Domain.Common;

namespace GestaoColaboradores.Domain.Entities;

public class Colaborador : CadastroBase
{
    public string Nome { get; set; } = string.Empty;

    public int UnidadeId { get; set; }
    public Unidade Unidade { get; set; } = null!;

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;
}

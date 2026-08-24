using GestaoColaboradores.Domain.Common;

namespace GestaoColaboradores.Domain.Entities;

public class Usuario : CadastroBase
{
    public string Login { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public Colaborador? Colaborador { get; set; }
}

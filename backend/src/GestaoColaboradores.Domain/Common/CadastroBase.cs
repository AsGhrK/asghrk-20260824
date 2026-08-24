namespace GestaoColaboradores.Domain.Common;

/// <summary>
/// Base para todo cadastro do sistema que exige um código único informado no cadastro
/// (Usuario, Unidade, Colaborador compartilham essa regra).
/// </summary>
public abstract class CadastroBase : EntidadeBase
{
    public string Codigo { get; set; } = string.Empty;
}

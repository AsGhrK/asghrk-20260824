namespace GestaoColaboradores.Domain.Common;

public abstract class EntidadeBase
{
    public int Id { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
}

namespace GestaoColaboradores.Application.Common.Exceptions;

/// <summary>Entidade solicitada não existe (mapeia para 404).</summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}

/// <summary>Violação de restrição única, ex.: código/login já cadastrado (mapeia para 409).</summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message)
    {
    }
}

/// <summary>Violação de regra de negócio, ex.: unidade inativa (mapeia para 400).</summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
    }
}

/// <summary>Credenciais inválidas no login (mapeia para 401).</summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}

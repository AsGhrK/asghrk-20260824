using GestaoColaboradores.Application.DTOs.Unidades;
using GestaoColaboradores.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoColaboradores.Api.Controllers;

[Authorize]
public class UnidadesController : BaseApiController
{
    private readonly IUnidadeService _unidadeService;

    public UnidadesController(IUnidadeService unidadeService)
    {
        _unidadeService = unidadeService;
    }

    /// <summary>Cadastra uma unidade com código único e nome.</summary>
    [HttpPost]
    public async Task<ActionResult<UnidadeResponseDto>> Create([FromBody] UnidadeCreateDto dto)
    {
        var unidade = await _unidadeService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = unidade.Id }, unidade);
    }

    /// <summary>Ativa/inativa uma unidade. Unidade inativa não permite novos colaboradores.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UnidadeResponseDto>> UpdateStatus(int id, [FromBody] UnidadeUpdateDto dto)
    {
        var unidade = await _unidadeService.UpdateStatusAsync(id, dto);
        return Ok(unidade);
    }

    /// <summary>Lista todas as unidades com seus colaboradores relacionados.</summary>
    [HttpGet]
    public async Task<ActionResult<List<UnidadeResponseDto>>> List()
    {
        var unidades = await _unidadeService.ListAsync();
        return Ok(unidades);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UnidadeResponseDto>> GetById(int id)
    {
        var unidade = await _unidadeService.GetByIdAsync(id);
        return Ok(unidade);
    }
}

using GestaoColaboradores.Application.DTOs.Colaboradores;
using GestaoColaboradores.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoColaboradores.Api.Controllers;

[Authorize]
public class ColaboradoresController : BaseApiController
{
    private readonly IColaboradorService _colaboradorService;

    public ColaboradoresController(IColaboradorService colaboradorService)
    {
        _colaboradorService = colaboradorService;
    }

    /// <summary>Cadastra um colaborador vinculado a uma unidade (ativa) e a um usuário.</summary>
    [HttpPost]
    public async Task<ActionResult<ColaboradorResponseDto>> Create([FromBody] ColaboradorCreateDto dto)
    {
        var colaborador = await _colaboradorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = colaborador.Id }, colaborador);
    }

    /// <summary>Atualiza um colaborador — nome e unidade podem ser alterados.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ColaboradorResponseDto>> Update(int id, [FromBody] ColaboradorUpdateDto dto)
    {
        var colaborador = await _colaboradorService.UpdateAsync(id, dto);
        return Ok(colaborador);
    }

    /// <summary>Remove um colaborador do sistema.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _colaboradorService.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>Lista todos os colaboradores com código, nome e unidade associada.</summary>
    [HttpGet]
    public async Task<ActionResult<List<ColaboradorResponseDto>>> List()
    {
        var colaboradores = await _colaboradorService.ListAsync();
        return Ok(colaboradores);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ColaboradorResponseDto>> GetById(int id)
    {
        var colaborador = await _colaboradorService.GetByIdAsync(id);
        return Ok(colaborador);
    }
}

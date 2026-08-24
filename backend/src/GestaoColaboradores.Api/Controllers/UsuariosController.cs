using GestaoColaboradores.Application.DTOs.Usuarios;
using GestaoColaboradores.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoColaboradores.Api.Controllers;

[Authorize]
public class UsuariosController : BaseApiController
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>Cadastra um usuário com código único, login, senha e status.</summary>
    [HttpPost]
    public async Task<ActionResult<UsuarioResponseDto>> Create([FromBody] UsuarioCreateDto dto)
    {
        var usuario = await _usuarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
    }

    /// <summary>Atualiza um usuário — apenas senha e status podem ser alterados.</summary>
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> Update(int id, [FromBody] UsuarioUpdateDto dto)
    {
        var usuario = await _usuarioService.UpdateAsync(id, dto);
        return Ok(usuario);
    }

    /// <summary>Lista todos os usuários, opcionalmente filtrando por status.</summary>
    [HttpGet]
    public async Task<ActionResult<List<UsuarioResponseDto>>> List([FromQuery] bool? ativo)
    {
        var usuarios = await _usuarioService.ListAsync(ativo);
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        return Ok(usuario);
    }
}

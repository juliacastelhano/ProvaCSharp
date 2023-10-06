using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FolhaCSharp.Models;
using FolhaCsharp.Data;


namespace FolhaCSharp.Controllers;


[ApiController]
[Route("api/funcionario")]
public class FuncionarioController : ControllerBase
{
    private readonly AppDataContext _ctx;
    public FuncionarioController(AppDataContext ctx)
    {
        _ctx = ctx;
    }


    [HttpGet]
    [Route("listar")]
    public ActionResult Listar()
    {
        try
        {
            List<Funcionario> categorias = _ctx.Funcionarios.ToList();
            return categorias.Count == 0 ? NotFound() : Ok(categorias);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }


    [HttpPost]
    [Route("cadastrar")]
    public ActionResult Cadastrar([FromBody] Funcionario funcionario)
    {
        try
        {
            _ctx.Funcionarios.Add(funcionario);
            _ctx.SaveChanges();
            return Created("", funcionario);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


}

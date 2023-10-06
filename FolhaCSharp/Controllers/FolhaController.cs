using FolhaCsharp.Data;
using FolhaCSharp.DTOs;
using FolhaCSharp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FolhaCSharp.Controllers;

[ApiController]
[Route("api/folha")]
public class FolhaController : ControllerBase
{
	/* 
	
	LISTAR api/folha/listar
	CADASTRAR api/folha/cadastrar
	BUSCAR api/folha/buscar/{cpf}/{mes}/{ano}
	
	*/

	private readonly AppDataContext _ctx;
	public FolhaController(AppDataContext ctx)
	{
		_ctx = ctx;
	}


	[HttpGet]
	[Route("listar")]
	public IActionResult Listar()
	{

		try
		{
			List<Folha> folhas =
				_ctx.Folhas.
				Include(x => x.Funcionario).
				ToList();

			return folhas.Count == 0 ? NotFound() : Ok(folhas);
		}
		catch (Exception e)
		{
			return NotFound(e.Message);
		}
	}


[HttpPost]
[Route("cadastrar")]
public IActionResult Cadastrar([FromBody] FolhaDTO folhaDTO)
{
	try
	{
		Funcionario? funcionario = _ctx.Funcionarios.Find(folhaDTO.FuncionarioId);
		if (funcionario == null)
		{
			return NotFound();
		}


		// cálculos aqui
		double salarioBruto = folhaDTO.Quantidade * folhaDTO.Valor;
		double irrf, inss, fgts, liquido;


		// Cálculo do IRRF
		if (salarioBruto > 4664.68)
		{
			irrf = (salarioBruto * 0.275) - 869.36;
		}
		else if (salarioBruto > 3751.06)
		{
			irrf = (salarioBruto * 0.225) - 636.13;
		}
		else if (salarioBruto > 2826.66)
		{
			irrf = (salarioBruto * 0.15) - 354.80;
		}
		else if (salarioBruto > 1903.99)
		{
			irrf = (salarioBruto * 0.075) - 142.80;
		}
		else
		{
			irrf = 0;
		}


		//  INSS
		if (salarioBruto > 5645.80)
		{
			inss = 621.03;
		}
		else if (salarioBruto > 2822.91)
		{
			inss = salarioBruto * 0.11;
		}
		else if (salarioBruto > 1623.73)
		{
			inss = salarioBruto * 0.09;
		}
		else
		{
			inss = salarioBruto * 0.08;
		}


		// FGTS
		fgts = salarioBruto * 0.08;


		//  líquido
		liquido = salarioBruto - (irrf + inss);


		// Crie uma nova instância de Folha com os valores calculados
		Folha folha = new Folha
		{
			Quantidade = folhaDTO.Quantidade,
			Mes = folhaDTO.Mes,
			Ano = folhaDTO.Ano,
			Valor = folhaDTO.Valor,
			Bruto = salarioBruto,
			Irff = irrf,
			Inss = inss,
			Fgts = fgts,
			Liquido = liquido,
			Funcionario = funcionario,
			FuncionarioId = folhaDTO.FuncionarioId
		};


		//  salvar
		_ctx.Folhas.Add(folha);
		_ctx.SaveChanges();


		return Created("", folha);
	}
	catch (Exception e)
	{
		Console.WriteLine(e);
		return BadRequest(e.Message);
	}
}


	[HttpGet]
	[Route("buscar/{cpf}/{mes}/{ano}")]
	public IActionResult Buscar([FromRoute] string cpf, int mes, int ano)
	{
		try
		{
			
			Folha? folhaCadastrada = _ctx.Folhas
    		.Include(x => x.Funcionario)
    		.FirstOrDefault(x => x.Funcionario.CPF == cpf && x.Mes == mes && x.Ano == ano);
			
			// Folha? folhaCadastrada = _ctx.Folhas.FirstOrDefault(x => x.Funcionario.CPF == cpf
			// && x.Mes == mes && x.Ano == ano);

			if (folhaCadastrada != null)
			{
				return Ok(folhaCadastrada);
			}
			return NotFound();
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}
	}
}
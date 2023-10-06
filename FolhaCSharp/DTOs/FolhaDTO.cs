using System.ComponentModel.DataAnnotations;
using FolhaCSharp.Models;


namespace FolhaCSharp.DTOs;
public class FolhaDTO
{
	[Required]
   
	public int Quantidade { get; set; }
	public int Mes { get; set; }
	public int Ano { get; set; }
	[Range(1, 1000)]
	public double Valor { get; set; }
	public double Bruto { get; set; }
	public double Irff { get; set; }
	public double Inss { get; set; }
	public double Fgts { get; set; }
	public double Liquido { get; set; }
	public int FuncionarioId { get; set; }
	public string? FuncionarioCpf { get; set; }
	public Funcionario? Funcionario { get; set; }
}

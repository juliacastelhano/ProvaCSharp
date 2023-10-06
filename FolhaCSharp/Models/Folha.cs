namespace FolhaCSharp.Models;
public class Folha
{
   
	public int FolhaId { get; set; }
	public double Valor { get; set; }
	public int Quantidade { get; set; }
	public int Mes { get; set; }
	public int Ano { get; set; }
	public Funcionario? Funcionario { get; set; }
	public int FuncionarioId { get; set; }
	
	public double Bruto { get; set; }
	
	public double Irff { get; set; }
	
	public double Inss { get; set; }
	
	public double Fgts { get; set; }
	
	public double Liquido { get; set; }
}

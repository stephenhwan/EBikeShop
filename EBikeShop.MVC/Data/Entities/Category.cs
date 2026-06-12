using System.ComponentModel.DataAnnotations;
namespace EBikeShop.MVC.Data.Entities;

public class Category		
{
	public Category()
	{
		Id = Guid.NewGuid();	
	}
	public Guid Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string? Description { get; set; }
	public int Position { get; set; }
}

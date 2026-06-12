using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EBikeShop.MVC.ViewModels
{
	[Bind("Id, Name, Description")]
	public class CategoryVM
	{
		public Guid Id { get; set; }

		[MaxLength(150, ErrorMessage = " Không được nhập quá 150 ký tự nha mấy cha nội")]
		public string Name { get; set; } = string.Empty;

		[MaxLength(5000)]
		public string? Description { get; set; }

		public int Position { get; set; }
	}
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace EBikeShop.MVC.ViewModels
{
[Bind("Id, FileName, StoredFileName, FilePath, FileType, FileSize, BikeMedias")]
	public class MediaVM
	{
		public Guid Id { get; set; }
		public string FileName { get; set; } = string.Empty;
		public string StoredFileName { get; set; } = string.Empty;
		public List<IFormFile> Files { get; set; } = new();
		public string? FilePath { get; set; }
		public string? FileType { get; set; }
		public long FileSize { get; set; }

	}
}

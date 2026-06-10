using Microsoft.AspNetCore.Mvc;

namespace EBikeShop.MVC.Data.Entities
{
	public class Media
	{
		public Guid Id { get; set; }
		public string FileName {get; set;} = string.Empty;
		public string StoredFileName {get; set;} = string.Empty;
		public string? FilePath {get; set;} 
		public string? FileType {get; set; }
		public long FileSize {get; set; }
		public ICollection<BikeMedia> BikeMedias { get; set; } = new List<BikeMedia>();

	}
}

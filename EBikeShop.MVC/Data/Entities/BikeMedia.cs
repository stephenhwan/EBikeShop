namespace EBikeShop.MVC.Data.Entities
{
	public class BikeMedia
	{
		public Guid Id { get; set; }
		public Guid BikeId { get; set; }   // ✅ Guid
		public Bike Bike { get; set; } = null!;
		public Guid MediaId { get; set; }  // ✅ Guid
		public Media Media { get; set; } = null!;
	}
}

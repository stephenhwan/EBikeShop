namespace EBikeShop.MVC.Data.Entities
{
	public class BikeMedia
	{
	public Guid Id { get; set; }

	public Guid BikeId { get; set; }
	public Bike Bike { get; set; } = null!;
	public Guid MediaId { get; set; }
	public Media Media { get; set; } = null!;
	}
}

using QRCoder;
using PollBuilder.MVC.Services.Interfaces;
namespace PollBuilder.MVC.Services.Implementations

{
	public class QRCodeService : IQRCodeService
	{
		public string Generate(string text)
		{
			using QRCodeGenerator generator = new();

			QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

			PngByteQRCode qr = new(data);

			//return qr.GetGraphic(20);
			var bytes = qr.GetGraphic(12);
			return Convert.ToBase64String(bytes);
		}
	}
}

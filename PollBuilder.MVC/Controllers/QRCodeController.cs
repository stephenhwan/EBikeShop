using Microsoft.AspNetCore.Mvc;
using PollBuilder.MVC.Services.Interfaces;

namespace PollBuilder.MVC.Controllers
{
	public class QRCodeController : Controller
	{
		private readonly IQRCodeService _serviceQRCode;

		public QRCodeController(IQRCodeService serviceQRCode)
		{
			_serviceQRCode = serviceQRCode;
		}

		public IActionResult Generate()
		{
			return View();
		}

		[HttpPost]
		//[ValidateAntiForgeryToken]
		//public IActionResult Generate(QRCodeVM qRCodeVM)
		public IActionResult Generate(string text)
		{
			var stringImage = _serviceQRCode.Generate(text);

			//return File(bytes, "image/png");

			//var fileQRCode = File(bytes, "image/png");
			//return RedirectToAction(nameof(QRCode), fileQRCode);
			return Content(stringImage);
		}

		//public IActionResult QRCode(byte[] fileQRCode)
		public IActionResult QRCode(FileContentResult fileQRCode)
		{
			return View(fileQRCode);
		}
	}
}

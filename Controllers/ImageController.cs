using Microsoft.AspNetCore.Mvc;

namespace ImageHosting.Controllers
{
	public class ImageController : Controller
	{
		public async Task<IActionResult> Upload()
		{
			return View();
		}
	}
}

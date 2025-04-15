using ImageHosting.Interfaces;

namespace ImageHosting.Services
{
	public class UrlService : IUrlService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public UrlService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public string GetBaseUrl()
		{
			var request = _httpContextAccessor.HttpContext?.Request;
			return request != null ? $"{request.Scheme}://{request.Host}" : "";
		}

		public string GetImageUrl(string guid)
		{
			return $"{GetBaseUrl()}/api/image/get/{guid}";
		}
	}

}

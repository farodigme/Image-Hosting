namespace ImageHosting.Interfaces
{
	public interface IUrlService
	{
		string GetBaseUrl();
		string GetImageUrl(string guid);
	}

}

namespace Shop.Testing.Helpers;

public static class TestConfiguration
{
	private static IConfigurationRoot _configuration;
	public static IConfigurationRoot GetConfiguration()
	{
		if (_configuration != null)
		{
			return _configuration;
		}
		_configuration = new ConfigurationBuilder()
		.SetBasePath(AppContext.BaseDirectory)
		.AddJsonFile("appsettings.json")
		.Build();

		return _configuration;
	}

}

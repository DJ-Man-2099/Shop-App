using System;

namespace Shop.Testing;

public class BaseIntegrationTest : IClassFixture<BaseWebAppFactory>
{
	protected readonly BaseWebAppFactory _factory;
	protected readonly HttpClient _client;


	public BaseIntegrationTest(BaseWebAppFactory factory)
	{
		_factory = factory;
		_client = _factory.CreateClient();
	}
}

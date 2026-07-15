using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;

namespace DEXOS.API.IntegrationTests;

public class ApiSqlDockerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiSqlDockerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Health_And_SqlServer_Connection_Should_Be_Ok_When_Docker_Tests_Enabled()
    {
        var runDockerTests = string.Equals(Environment.GetEnvironmentVariable("RUN_DOCKER_TESTS"), "true", StringComparison.OrdinalIgnoreCase);
        if (!runDockerTests)
        {
            return;
        }

        var apiClient = _factory.CreateClient();
        var healthResponse = await apiClient.GetAsync("/health");
        Assert.True(healthResponse.IsSuccessStatusCode);

        var connectionString = "Server=localhost,14333;Database=DEXOS;User Id=sa;Password=Your_strong_password123!;TrustServerCertificate=True;Encrypt=False;";
        await using var sqlConnection = new SqlConnection(connectionString);
        await sqlConnection.OpenAsync();

        Assert.Equal(System.Data.ConnectionState.Open, sqlConnection.State);
    }
}

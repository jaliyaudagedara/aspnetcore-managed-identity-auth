using Azure.Core;
using Azure.Identity;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/claims", async (IConfiguration configuration) =>
{
    DefaultAzureCredential credential = new();

    string[] scopes = configuration.GetSection("InternalApi:Scopes").Get<string[]>()!;
    TokenRequestContext tokenRequestContext = new(scopes);
    AccessToken accessToken = await credential.GetTokenAsync(tokenRequestContext);

    HttpClient httpClient = new()
    {
        DefaultRequestHeaders =
        {
            Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Token)
        }
    };

    string internalApiUrl = configuration.GetValue<string>("InternalApi:BaseUrl")!;
    string result = await httpClient.GetStringAsync($"{internalApiUrl}/claims");
    return JsonNode.Parse(result);
});

app.Run();
using System.Net.Http.Json;
using CommonLibrary.Models;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace CommonLibrary.ServerRegistry
{
    public class ServerRegistryClient
    {
        private const string RegisterUrl = "Auth/Server/Register";
        private const string AuthType = "Bearer";

        private readonly HttpClient client;
        private readonly ServerRegistryOptions options;

        public ServerRegistryClient(HttpClient client, IOptions<ServerRegistryOptions> options)
        {
            client.BaseAddress = new(options.Value.AuthServerAddress);
            client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"{AuthType} {options.Value.Token}");
            this.client = client;
            this.options = options.Value;
        }

        public async Task RegisterServerAsync()
        {
            using var response = await client.PostAsJsonAsync(
                RegisterUrl,
                new RegisterArguments(
                    options.Name,
                    options.Role,
                    options.Address,
                    options.Description,
                    options.ExpireSec));

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Server Register fail. status code {response.StatusCode}");
            }
        }

        private sealed class RegisterArguments
        {
            public RegisterArguments(string name, ServerRoles role, string address, string description, long expireSec)
            {
                Name = name;
                Role = role;
                Address = address;
                Description = description;
                ExpireSec = expireSec;
            }

            public string Name { get; init; } = null!;

            public ServerRoles Role { get; init; }

            public string Address { get; init; } = null!;

            public string Description { get; init; } = null!;

            public long ExpireSec { get; init; }
        }
    }
}
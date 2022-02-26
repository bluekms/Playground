using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommonLibrary.Models;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace WorldServer.Services
{
    public class ServerRegistryClient
    {
        private readonly HttpClient client;
        private readonly ServerRegistryOptions options;

        public ServerRegistryClient(HttpClient client, IOptions<ServerRegistryOptions> options)
        {
            this.client = client;
            this.options = options.Value;

            this.client.BaseAddress = new Uri(this.options.Address);
            this.client.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"SessionId {this.options.Token}");
        }

        public async Task RegisterServerAsync()
        {
            //using var res = await client.PostAsJsonAsync("", )
        }
    }
}
using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient();

            var disco = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;

            if(disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "api1"
            }).Result;

            if(tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;                
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            var apiClient = new HttpClient();
            apiClient.SetBearerToken(tokenResponse.AccessToken);

            var response = apiClient.GetAsync("http://localhost:5001/identity").Result;
            if(!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(JArray.Parse(content));
            }

            // Console.WriteLine("Hello World!");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace UsuariosAleatorios
{
    public class RandomUserResponse
    {
        public List<RandomUser> Results { get; set; }
    }

    public class RandomUser
    {
        public Name Name { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public Location Location { get; set; }
    }

    public class Name
    {
        public string Title { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
    }

    public class Location
    {
        public string Country { get; set; }
    }

    public interface IRandomUserService
    {
        Task<List<RandomUser>> GetRandomUsersAsync(int count);
    }

    public class RandomUserService : IRandomUserService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://randomuser.me/api/";

        public RandomUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RandomUser>> GetRandomUsersAsync(int count)
        {
            // Aquí se utiliza el parámetro "results" para solicitar todos los usuarios en una sola llamada
            HttpResponseMessage response = await _httpClient.GetAsync(ApiUrl + $"?results={count}");
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            RandomUserResponse userResponse = JsonSerializer.Deserialize<RandomUserResponse>(json, options);
            return userResponse.Results;
        }
    }

    class Program
    {
        static async Task Main()
        {
            using HttpClient httpClient = new HttpClient();
            IRandomUserService userService = new RandomUserService(httpClient);

            bool continuar = true;
            while (continuar)
            {
                Console.Write("¿Cuántos usuarios aleatorios deseas obtener? ");
                if (!int.TryParse(Console.ReadLine(), out int cantidad) || cantidad < 1)
                {
                    Console.WriteLine("Entrada no válida. Intenta nuevamente.");
                    continue;
                }

                try
                {
                    Console.WriteLine("Obteniendo usuarios...");
                    List<RandomUser> usuarios = await userService.GetRandomUsersAsync(cantidad);
                    Console.WriteLine("\nUsuarios obtenidos:");
                    foreach (var usuario in usuarios)
                    {
                        string nombreCompleto = $"{usuario.Name.Title} {usuario.Name.First} {usuario.Name.Last}";
                        Console.WriteLine($"Nombre: {nombreCompleto} | Género: {usuario.Gender} | Email: {usuario.Email} | País: {usuario.Location.Country}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine("Error de conexión: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inesperado: " + ex.Message);
                }

                Console.Write("\n¿Deseas obtener más usuarios? (s/n): ");
                continuar = Console.ReadLine().Trim().ToLower() == "s";
                Console.WriteLine();
            }
        }
    }
}

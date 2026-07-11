

using Application.DTO_s.EntityDTO.Libros;
using Application.DTO_s.LoginDTO;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Pruebas_Integracoin.Libros_Test
{
  
    public class GetLibroTest : IClassFixture<WebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _client;

        public GetLibroTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            var loginRequest = new Authenticationrequest
            {
                UserName = "RubenDextra",
                PassWord = "123Pa$$word!"
            };

            var loginResponse = await _client.PostAsJsonAsync(
                "/api/Login/Login-user",
                loginRequest);

            loginResponse.EnsureSuccessStatusCode();

            var loginResult = await loginResponse.Content
                .ReadFromJsonAsync<AuthenticationResponse>();

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    loginResult!.JWToken);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

     
        [Fact]
        public async Task Agrega_un_Libro_La_Respuesta_Deberia_ser_200Ok()
        {
            // Arrange
            var request = new SaveLibroDTO
            {
                Titulo = "El Corones no tiene quien le escriba",
                Autor_id = 1,
                Año_publicacion = 2002,
                Genero = "Historia"

            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "api/Libros",
                request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        }

        [Fact]
        public async Task Agrega_un_Libro_La_Respuesta_Deberia_ser_404NotFound()
        {
            // Arrange
            var request = new SaveLibroDTO
            {
                Titulo = "El Corones no tiene quien le escriba",
                Autor_id = 45458,
                Año_publicacion = 2002,
                Genero = "Historia"

            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "api/Libros",
                request);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);


        }

        [Fact]
        public async Task Agrega_un_Libro_La_Respuesta_Deberia_ser_400BadRequest()
        {
            // Arrange
            var request = new SaveLibroDTO
            {
                Titulo = "El Corones no tiene quien le escriba",
                Autor_id = 1,
                Año_publicacion = -2002,
                Genero = "Historia"

            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "api/Libros",
                request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }
        [Fact]
        public async Task Get_Libros_antes_de_2000()
        {
            // Arrange


            // Act
            var response = await _client.GetAsync(
                "api/Libros/Get_Libros");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);


        }




    }
}

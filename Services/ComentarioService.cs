using System.Net.Http.Json;
using RecetArreWeb.DTOs;

namespace RecetArreWeb.Services
{
    public interface IComentarioService
    {
        Task<List<ComentarioDto>> ObtenerPorReceta(int recetaId);
        Task<ComentarioDto?> Crear(ComentarioCreacionDto comentarioDto);
        Task<bool> Actualizar(int id, ComentarioModificacionDto comentarioDto);
        Task<bool> Eliminar(int id);
        Task<(double Promedio, int Total)> ObtenerRating(int recetaId); 
    }

    public class ComentarioService : IComentarioService
    {
        private readonly HttpClient httpClient;
        private const string endpoint = "api/Comentarios";

        public ComentarioService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<List<ComentarioDto>> ObtenerPorReceta(int recetaId)
        {
            try
            {
                var comentarios = await httpClient.GetFromJsonAsync<List<ComentarioDto>>($"{endpoint}/receta/{recetaId}");
                return comentarios ?? new List<ComentarioDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener comentarios: {ex.Message}");
                return new List<ComentarioDto>();
            }
        }

        public async Task<ComentarioDto?> Crear(ComentarioCreacionDto comentarioDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync(endpoint, comentarioDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<ComentarioDto>();
                }

                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear comentario: {error}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear comentario: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> Actualizar(int id, ComentarioModificacionDto comentarioDto)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync($"{endpoint}/{id}", comentarioDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar comentario {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar comentario {id}: {ex.Message}");
                return false;
            }
        }
        public async Task<(double Promedio, int Total)> ObtenerRating(int recetaId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{endpoint}/receta/{recetaId}/rating");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<RatingResponse>();
                    return (result?.Promedio ?? 0, result?.Total ?? 0);
                }
                return (0,0);
            }
            catch
            {
                return(0,0);
            }
        }
    }
    //Clase para deserializar la respuesta ah ya, básicamente:
    //Con esta función se toman los datos en JSON con su formato y los parsea a un objeto del tipo RatingResponse
    public class RatingResponse
    {
        public double Promedio {get;set;}
        public int Total {get;set;}
    }
}

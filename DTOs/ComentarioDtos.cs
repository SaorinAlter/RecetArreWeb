using System.Text.Json.Serialization; //Using para serializar el JSON
using System.ComponentModel.DataAnnotations;
namespace RecetArreWeb.DTOs
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string? Contenido { get; set; }
        [JsonPropertyName("puntuacion")]
        public int? Puntuacion {get;set;} //Añadimos esto al ya sabes
        public DateTime CreadoUtc { get; set; }
        public int RecetaId { get; set; }
        public string UsuarioId { get; set; } = default!;
        [JsonPropertyName("nombreUsuario")]
        public string NombreUsuario {get; set; } = default!; //si truena por algo sospecho que será por esto
    }

    public class ComentarioCreacionDto
    {
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "El comentario debe tener entre 1 y 1000 caracteres")]
        public string? Contenido { get; set; }

        [Range(1,5, ErrorMessage = "La puntuación debe ser entre 1 y 5")]
        public int? Puntuacion {get; set;}

        [Required]
        public int RecetaId { get; set; }
    }

    public class ComentarioModificacionDto
    {
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "El comentario debe tener entre 1 y 1000 caracteres")]
        public string? Contenido { get; set; }
        [Range(1, 5, ErrorMessage = "La puntuación debe ser entre 1 y 5")]
        public int? Puntuacion {get;set;}
    }
}

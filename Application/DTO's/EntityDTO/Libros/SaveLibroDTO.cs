

using System.ComponentModel.DataAnnotations;

namespace Application.DTO_s.EntityDTO.Libros
{
    public class SaveLibroDTO
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un autor válido.")]
        public int Autor_id { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        [Range(1000, 2100, ErrorMessage = "Debe ingresar un año de publicación válido.")]
        public int Año_publicacion { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(100, ErrorMessage = "El género no puede exceder los 100 caracteres.")]
        public string Genero { get; set; } = string.Empty;
    }
}

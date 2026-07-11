using Application.DTO_s.EntityDTO.Libros;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly ILibrosService _librosService;
        public LibrosController(ILibrosService librosService)
        {
            _librosService = librosService;
        }

        /// <summary>
        /// Obtiene la lista de libros publicados antes del año 2000.
        /// </summary>
        /// <returns>Lista de libros encontrados.</returns>
        /// <response code="200">Lista de libros obtenida correctamente.</response>
        /// <response code="204">No existen libros que cumplan el criterio de búsqueda.</response>
        /// <response code="500">Error interno del servidor.</response>
        /// 
        [HttpGet("Get_Libros")]
        [ProducesResponseType(typeof(List<LibrosDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LibrosDTO>>> Get_Libros()
        {
            var result = await _librosService.GetLibrosPostedbefore2000();
            return result == null || !result.Any() ? NoContent() : Ok(result);
        }

        /// <summary>
        /// Registra un nuevo libro en el sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite crear un nuevo libro asociándolo a un autor existente.
        /// El autor especificado mediante <c>Autor_id</c> debe existir previamente en el sistema.
        ///
        /// Ejemplo de solicitud:
        ///
        ///     POST /api/libros
        ///     {
        ///         "titulo": "Cien años de soledad",
        ///         "autor_id": 1,
        ///         "año_publicacion": 1967,
        ///         "genero": "Realismo mágico"
        ///     }
        ///
        /// </remarks>
        /// <param name="saveLibroDTO">
        /// Objeto que contiene la información necesaria para registrar el libro.
        /// </param>
        /// <returns>
        /// Retorna la información del libro registrado.
        /// </returns>
        /// <response code="200">
        /// El libro fue registrado correctamente.
        /// </response>
        /// <response code="400">
        /// La solicitud contiene datos inválidos o incompletos.
        /// </response>
        /// <response code="404">
        /// El autor especificado no existe.
        /// </response>
        /// <response code="500">
        /// Ocurrió un error interno mientras se procesaba la solicitud.
        /// </response>
        /// 

        [Authorize(Roles ="Admin")]
        [HttpPost]
        [ProducesResponseType(typeof(SaveLibroDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SaveLibroDTO>> AddLibro([FromBody] SaveLibroDTO saveLibroDTO)
        {
            return Ok(await _librosService.AddLibro(saveLibroDTO));
        }

    }
}

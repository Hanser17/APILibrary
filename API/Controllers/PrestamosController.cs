using Application.DTO_s.EntityDTO.Libros;
using Application.DTO_s.EntityDTO.Prestamo;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrestamosController : Controller
    {
        private readonly IPrestamoServices _prestamoServices;
        public PrestamosController(IPrestamoServices prestamoServices)
        {
            _prestamoServices = prestamoServices;
        }

        /// <summary>
        /// Obtiene el listado de libros que actualmente se encuentran prestados y aún no han sido devueltos.
        /// </summary>
        /// <remarks>
        /// Un libro se considera pendiente de devolución cuando el préstamo asociado
        /// no posee una fecha de devolución registrada.
        /// </remarks>
        /// <returns>
        /// Retorna una colección de libros con préstamos activos pendientes de devolución.
        /// </returns>
        /// <response code="200">
        /// La consulta fue procesada correctamente y se encontraron libros pendientes de devolución.
        /// </response>
        /// <response code="204">
        /// No existen libros con préstamos pendientes de devolución.
        /// </response>
        /// <response code="500">
        /// Ocurrió un error interno mientras se procesaba la solicitud.
        /// </response>
        /// 
        
        [HttpGet("Get_Libros_No_Devueltos")]
        [ProducesResponseType(typeof(List<LibrosNoDevueltosDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<LibrosNoDevueltosDTO>>> Get_Libros_No_Devueltos()
        {
            var result = await _prestamoServices.GetLibrosNoDevueltos();

            return result == null || !result.Any()
                ? NoContent()
                : Ok(result);
        }

        /// <summary>
        /// Actualiza la fecha de devolución de un préstamo existente.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite registrar la devolución de un libro asociado a un préstamo.
        /// La actualización se realiza mediante el identificador del préstamo y la fecha en la que
        /// fue devuelto el libro.
        ///
        /// Ejemplo de solicitud:
        ///
        ///     PUT /api/prestamos
        ///     {
        ///         "id": 1,
        ///         "fecha_devolucion": "2026-07-10T00:00:00"
        ///     }
        ///
        /// </remarks>
        /// <param name="DTO">
        /// Objeto que contiene el identificador del préstamo y la fecha de devolución registrada.
        /// </param>
        /// <returns>
        /// Retorna la información actualizada del préstamo.
        /// </returns>
        /// <response code="200">
        /// La fecha de devolución del préstamo fue actualizada correctamente.
        /// </response>
        /// <response code="400">
        /// La información enviada no cumple con las validaciones requeridas.
        /// </response>
        /// <response code="404">
        /// No se encontró un préstamo con el identificador proporcionado.
        /// </response>
        /// <response code="500">
        /// Ocurrió un error interno al procesar la solicitud.
        /// </response>
        /// 
        [Authorize(Roles ="Admin")]
        [HttpPut]
        [ProducesResponseType(typeof(PrestamoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PrestamoDTO>> Update_fecha_devolucion_Prestamo(
            [FromBody] Update_fecha_devolucion_Prestamo_DTO DTO)
        {
            return await _prestamoServices.Update_fecha_devolucion_Prestamo(DTO);
        }

        /// <summary>
        /// Elimina un préstamo existente del sistema.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite eliminar un registro de préstamo utilizando su identificador.
        /// Antes de realizar la eliminación se valida que el préstamo exista.
        /// 
        /// Ejemplo de solicitud:
        ///
        ///     DELETE /api/prestamos/1
        ///
        /// </remarks>
        /// <param name="id">
        /// Identificador único del préstamo que será eliminado.
        /// </param>
        /// <returns>
        /// Retorna un mensaje indicando el resultado de la operación.
        /// </returns>
        /// <response code="200">
        /// El préstamo fue eliminado correctamente.
        /// </response>
        /// <response code="400">
        /// Ocurrió un error al intentar eliminar el préstamo.
        /// </response>
        /// <response code="404">
        /// No se encontró un préstamo con el identificador proporcionado.
        /// </response>
        /// <response code="500">
        /// Ocurrió un error interno al procesar la solicitud.
        /// </response>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeletePrestamo(int id)
        {
            var exit = await _prestamoServices.BorrarPrestamo(id);

            return exit
                ? Ok("Préstamo borrado exitosamente")
                : BadRequest("Ocurrió un error inesperado");
        }
    }
}

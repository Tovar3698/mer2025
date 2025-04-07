using Business;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Utilities.Exceptions;


namespace Webs.Controllers
{
    /// <summary>
    /// Controlador para la gestión de permisos en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TypeCitationController : ControllerBase
    {
        private readonly TypeCitationBusiness _TypeCitationBusiness;
        private readonly ILogger<TypeCitationController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos
        /// </summary>
        /// <param name="FormBusiness">Capa de negocio de permisos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public TypeCitationController(TypeCitationBusiness TypeCitationBusiness, ILogger<TypeCitationController> logger)
        {
            _TypeCitationBusiness = TypeCitationBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema
        /// </summary>
        /// <return>Lista de permisos</return>
        /// <response code="200">Retornar la lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TypeCitationDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var typecitation = await _TypeCitationBusiness.GetAllTypeCitationAsync();
                return Ok(typecitation);
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permisos");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene un permiso específico por su ID
        /// </summary>
        /// <param name="id">ID del permiso</param>
        /// <returns>Permiso solicitado</returns>
        /// <response code="200">Retorna el permiso solicitado</response>
        /// <response code="400">ID proporcionado no válido</response>
        /// <response code="404">Permiso no encontrado</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TypeCitationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTypeCitationById(int id)
        {
            try
            {
                var TypeCitation = await _TypeCitationBusiness.GetTypeCitationByIdAsync(id);
                return Ok(TypeCitation);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {FormId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {FormId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {FormId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(TypeCitationDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateTypeCitation([FromBody] TypeCitationDto typecitationDto)
        {
            try
            {
                var createTypeCitation = await _TypeCitationBusiness.CreateTypeCitationAsync(typecitationDto);
                return CreatedAtAction(nameof(GetTypeCitationById), new { id = createTypeCitation.Id }, createTypeCitation);

            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "validacion fallida al cear Usuario");
                return BadRequest(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al crear Usuario");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}



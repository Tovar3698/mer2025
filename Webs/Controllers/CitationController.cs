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
    public class CitationController : ControllerBase
    {
        private readonly CitationBusiness _CitationBusiness;
        private readonly ILogger<CitationController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos
        /// </summary>
        /// <param name="CitationBusiness">Capa de negocio de permisos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public CitationController(CitationBusiness CitationBusiness, ILogger<CitationController> logger)
        {
            _CitationBusiness = CitationBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema
        /// </summary>
        /// <return>Lista de permisos</return>
        /// <response code="200">Retornar la lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CitationDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllForm()
        {
            try
            {
                var citation = await _CitationBusiness.GetAllCitationAsync();
                return Ok(citation);
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
        [ProducesResponseType(typeof(CitationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetFormById(int id)
        {
            try
            {
                var Citation = await _CitationBusiness.GetPermissionByIdAsync(id);
                return Ok(Citation);
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
        [ProducesResponseType(typeof(CitationDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateForm([FromBody] CitationDto citationDto)
        {
            try
            {
                var createCitation = await _CitationBusiness.CreatePermissionAsync(citationDto);
                return CreatedAtAction(nameof(GetFormById), new { id = createCitation.Id }, createCitation);

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
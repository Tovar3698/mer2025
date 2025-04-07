using Business;
using Data;
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
    public class InfoDoctorController : ControllerBase
    {
        private readonly InfoDoctorBusiness _InfoDoctorBusiness;
        private readonly ILogger<InfoDoctorController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos
        /// </summary>
        /// <param name="ModuleBusiness">Capa de negocio de permisos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public InfoDoctorController(InfoDoctorBusiness InfoDoctorBusiness, ILogger<InfoDoctorController> logger)
        {
            _InfoDoctorBusiness = InfoDoctorBusiness;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los permisos del sistema
        /// </summary>
        /// <return>Lista de permisos</return>
        /// <response code="200">Retornar la lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InfoDoctorDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllInfoDoctor()
        {
            try
            {
                var infodoctor = await _InfoDoctorBusiness.GetAllInfoDoctorAsync();
                return Ok(infodoctor);
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
        [ProducesResponseType(typeof(InfoDoctorDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetInfoDoctorById(int id)
        {
            try
            {
                var InfoDoctor = await _InfoDoctorBusiness.GetPermissionByIdAsync(id);
                return Ok(InfoDoctor);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {RolId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {RolId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {RolId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(InfoDoctorDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateInfoDoctor([FromBody] InfoDoctorDto infodoctorDto)
        {
            try
            {
                var createInfoDoctor = await _InfoDoctorBusiness.CreatePermissionAsync(infodoctorDto);
                return CreatedAtAction(nameof(GetInfoDoctorById), new { id = createInfoDoctor.Id }, createInfoDoctor);

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
using Business;
using Entity.DTOs;
using Entity.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Exceptions;


namespace Webs.Controllers
{
    /// <summary>
    /// Controlador para la gestión de permisos en el sistema
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationBusiness _NotificationBusiness;
        private readonly ILogger<NotificationController> _logger;

        /// <summary>
        /// Constructor del controlador de permisos
        /// </summary>
        /// <param name="NotificationBusiness">Capa de negocio de permisos</param>
        /// <param name="logger">Logger para registro de eventos</param>
        public NotificationController(NotificationBusiness NotificationBusiness, ILogger<NotificationController> logger)
        {
            _NotificationBusiness = NotificationBusiness;
            _logger = logger;
        }


        /// <summary>
        /// Obtiene todos los permisos del sistema
        /// </summary>
        /// <return>Lista de permisos</return>
        /// <response code="200">Retornar la lista de permisos</response>
        /// <response code="500">Error interno del servidor</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NotificationDto>), 200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllNotification(NotificationBusiness _NotificationBusiness)
        {
            try
            {
                var Notification = await _NotificationBusiness.GetAllNotificationsAsync();
                return Ok(Notification);
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
        [ProducesResponseType(typeof(NotificationDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetNotificationById(int id, object Notification)
        {
            try
            {
                var notification = await _NotificationBusiness.GetRolByIdAsync(id);
                return Ok(notification);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validación fallida para el permiso con ID: {UserId}", id);
                return BadRequest(new { message = ex.Message });
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex, "Permiso no encontrado con ID: {UserId}", id);
                return NotFound(new { message = ex.Message });
            }
            catch (ExternalServiceException ex)
            {
                _logger.LogError(ex, "Error al obtener permiso con ID: {UserId}", id);
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        [ProducesResponseType(typeof(NotificationDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CreateNotification([FromBody] NotificationDto notificationDto)
        {
            try
            {
                var createNotification = await _NotificationBusiness.CreateNotificationAsync(notificationDto);
                return CreatedAtAction(nameof(GetNotificationById), new { id = createNotification.Id }, createNotification);

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

using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class NotificationBusiness
    {
        private readonly NotificationData _notificationData;
        private readonly ILogger<NotificationBusiness> _logger;

        public NotificationBusiness(NotificationData notificationData, ILogger<NotificationBusiness> logger)
        {
            _notificationData = notificationData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<NotificationDto>> GetAllNotificationsAsync()
        {
            try
            {
                var notifications = await _notificationData.GetAllAsync();
                var notificationsDto = new List<NotificationDto>();

                foreach (var notification in notifications)
                {
                    notificationsDto.Add(new NotificationDto
                    {
                        Id = notification.Id,
                        Name = notification.Name,
                        Description = notification.Description // Si existe en la entidad
                    });
                }

                return notificationsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<NotificationDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var notification = await _notificationData.GetByIdAsync(id);
                if (notification == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new NotificationDto
                {
                    Id = notification.Id,
                    Name = notification.Name,
                    Description = notification.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<NotificationDto> CreateNotificationAsync(NotificationDto NotificationDto)
        {
            try
            {
                ValidateNotification(NotificationDto);

                var notification = new Notification
                {
                    Name = NotificationDto.Name,
                    Description = NotificationDto.Description // Si existe en la entidad
                };

                var notificationCreado = await _notificationData.CreateAsync(notification);

                return new NotificationDto
                {

                    Id = notificationCreado.Id,


                    Name = notificationCreado.Name,
                    Description = notificationCreado.Description // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", NotificationDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateNotification(NotificationDto NotificationDto)
        {
            if (NotificationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(NotificationDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }// Método para mapear de Rol a RolDTO
        private NotificationDto MapToDTO(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                Name = notification.Name,
                Description = notification.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private Notification MapToEntity(NotificationDto notificationDto)
        {
            return new Notification
            {
                Id = notificationDto.Id,
                Name = notificationDto.Name,
                Description = notificationDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<NotificationDto> MapToDTOList(IEnumerable<Notification> notifications)
        {
            var notificationsDto = new List<NotificationDto>();
            foreach (var notification in notifications)
            {
                notificationsDto.Add(MapToDTO(notification));
            }
            return notificationsDto;
        }
    }
}
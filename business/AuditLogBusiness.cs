using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using Utilities.Exceptions;

namespace Business
{
    /// <summary>
    /// Clase de negocio encargada de la lógica relacionada con los roles del sistema.
    /// </summary>
    public class AuditLogBusiness
    {
        private readonly AuditLogData _auditlogData;
        private readonly ILogger<AuditLogBusiness> _logger;

        public AuditLogBusiness(AuditLogData auditlogData, ILogger<AuditLogBusiness> logger)
        {
            _auditlogData = auditlogData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<AuditLogDto>> GetAllAuditLogAsync()
        {
            try
            {
                var auditlog = await _auditlogData.GetAllAsync();
                var auditlogDto = MapToDTOList(auditlog);
                return auditlogDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<AuditLogDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var auditlog = await _auditlogData.GetByIdAsync(id);
                if (auditlog == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new AuditLogDto
                {
                    OldValue = auditlog.OldValue,
                    NewValue = auditlog.NewValue,
                    Action = auditlog.Action,
                    TableName = auditlog.TableName

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }
        // Método para crear un rol desde un DTO
        public async Task<AuditLogDto> CreatePermissionAsync(AuditLogDto AuditLogDto)
        {
            try
            {
                ValidateAuditLog(AuditLogDto);

                var auditlog = new AuditLog
                {
                    OldValue = AuditLogDto.OldValue,
                    NewValue = AuditLogDto.NewValue,
                    Action = AuditLogDto.Action,
                    TableName = AuditLogDto.TableName

                };

                var auditlogCreado = await _auditlogData.CreateAsync(auditlog);

                return new AuditLogDto
                {
                    OldValue = auditlogCreado.OldValue,
                    NewValue = auditlogCreado.NewValue,
                    Action = auditlogCreado.Action,
                    TableName = auditlogCreado.TableName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", AuditLogDto?.Id);
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateAuditLog(AuditLogDto AuditLogDto)
        {
            if (AuditLogDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(AuditLogDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }        // Método para mapear de Rol a RolDTO
        private AuditLogDto MapToDTO(AuditLog AuditLog)
        {
            return new AuditLogDto
            {
                OldValue = AuditLog.OldValue,
                NewValue = AuditLog.NewValue,
                Action = AuditLog.Action,
                TableName = AuditLog.TableName // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private AuditLog MapToEntity(AuditLogDto auditlogDto)
        {
            return new AuditLog
            {
                OldValue = auditlogDto.OldValue,
                NewValue = auditlogDto.NewValue,
                Action = auditlogDto.Action,
                TableName = auditlogDto.TableName // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<AuditLogDto> MapToDTOList(IEnumerable<AuditLog> auditlogs)
        {
            var AuditLogDto = new List<AuditLogDto>();
            foreach (var auditlog in auditlogs)
            {
                AuditLogDto.Add(MapToDTO(auditlog));
            }
            return AuditLogDto;
        }
    }
}




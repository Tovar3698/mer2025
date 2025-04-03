using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business
{
    public class RolFormPermissionBusiness
    {
        private readonly RolFormPermissionData _rolFormPermissionData;
        private readonly ILogger _logger;

        public RolFormPermissionBusiness(RolFormPermissionData rolFormPermissionData, ILogger logger)
        {
            _rolFormPermissionData = rolFormPermissionData;
            _logger = logger;
        }

        public async Task<IEnumerable<RolFormPermissionDto>> GetAllRolFormPermissionsAsync(RolFormPermissionDto dto)
        {
            try
            {
                var rolFormPermissionList = await _rolFormPermissionData.GetAllAsync();
                var permissionsDtoList = new List<RolFormPermissionDto>();

                foreach (var entity in rolFormPermissionList)
                {
                    permissionsDtoList.Add(new RolFormPermissionDto
                    {
                        Id = entity.Id,
                        RolId = entity.RolId,
                        FormId = entity.FormId,
                        PermissionId = entity.PermissionId
                    });
                }
                return permissionsDtoList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los permisos de formulario por rol");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de permisos", ex);
            }
        }

        public async Task<RolFormPermissionDto> GetRolFormPermissionByIdAsync(int permisoId)
        {
            if (permisoId <= 0)
            {
                _logger.LogWarning("Se intentó obtener un RolFormPermission con ID inválido: {id}", permisoId);
                throw new ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var permisoEntidad = await _rolFormPermissionData.GetByIdAsync(permisoId);
                if (permisoEntidad == null)
                {
                    _logger.LogInformation("No se encontró ningún RolFormPermission con ID: {id}", permisoId);
                    throw new EntityNotFoundException("RolFormPermission", permisoId);
                }

                return new RolFormPermissionDto
                {
                    Id = permisoEntidad.Id,
                    RolId = permisoEntidad.RolId,
                    FormId = permisoEntidad.FormId,
                    PermissionId = permisoEntidad.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el RolFormPermission con ID: {id}", permisoId);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el RolFormPermission con ID {permisoId}", ex);
            }
        }

        public async Task<RolFormPermissionDto> CreateRolFormPermissionAsync(RolFormPermissionDto nuevoPermisoDto)
        {
            try
            {
                ValidateRolFormPermission(nuevoPermisoDto);

                var nuevoPermisoEntidad = new RolFormPermission
                {
                    RolId = nuevoPermisoDto.RolId,
                    FormId = nuevoPermisoDto.FormId,
                    PermissionId = nuevoPermisoDto.PermissionId
                };

                var permisoCreado = await _rolFormPermissionData.CreateAsync(nuevoPermisoEntidad);

                return new RolFormPermissionDto
                {
                    Id = permisoCreado.Id,
                    RolId = permisoCreado.RolId,
                    FormId = permisoCreado.FormId,
                    PermissionId = permisoCreado.PermissionId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo permiso: {RolFormPermissionId}", nuevoPermisoDto?.Id ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el permiso", ex);
            }
        }

        private void ValidateRolFormPermission(RolFormPermissionDto permisoValidar)
        {
            if (permisoValidar == null)
            {
                throw new ValidationException("El objeto RolFormPermission no puede ser nulo");
            }
            if (permisoValidar.RolId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un RolFormPermission con RolId inválido");
                throw new ValidationException("RolId", "El RolId es obligatorio y debe ser mayor que cero");
            }
            if (permisoValidar.FormId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un RolFormPermission con FormId inválido");
                throw new ValidationException("FormId", "El FormId es obligatorio y debe ser mayor que cero");
            }
            if (permisoValidar.PermissionId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un RolFormPermission con PermissionId inválido");
                throw new ValidationException("PermissionId", "El PermissionId es obligatorio y debe ser mayor que cero");
            }
          }        // Método para mapear de Rol a RolDTO
        private RolFormPermissionDto MapToDTO(RolFormPermission RolFormPermission)
        {
            return new RolFormPermissionDto
            {
                Id = RolFormPermission.Id,
                Name = RolFormPermission.Name,
                Description = RolFormPermission.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private RolFormPermission MapToEntity(RolFormPermissionDto rolformpermissionDto)
        {
            return new RolFormPermission
            {
                Id = rolformpermissionDto.Id,
                Name = rolformpermissionDto.Name,
                Description = rolformpermissionDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<RolFormPermissionDto> MapToDTOList(IEnumerable<RolFormPermission> rolformpermissions)
        {
            var RolFormPermissionDto = new List<RolFormPermissionDto>();
            foreach (var rolformpermission in rolformpermissions)
            {
                RolFormPermissionDto.Add(MapToDTO(rolformpermission));
            }
            return RolFormPermissionDto;
        }
    }
}



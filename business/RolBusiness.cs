﻿using Data;
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
    public class RolBusiness
    {
        private readonly RolData _rolData;
        private readonly ILogger<RolBusiness> _logger;

        public RolBusiness(RolData rolData, ILogger<RolBusiness> logger)
        {
            _rolData = rolData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<RolDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await _rolData.GetAllAsync();
                var rolesDto = new List<RolDto>();

                foreach (var rol in roles)
                {
                    rolesDto.Add(new RolDto
                    {
                        Id = rol.Id,
                        Name = rol.Name,
                        Description = rol.Description // Si existe en la entidad
                    });
                }

                return rolesDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<RolDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var rol = await _rolData.GetByIdAsync(id);
                if (rol == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new RolDto
                {
                    Id = rol.Id,
                    Name = rol.Name,
                    Description = rol.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<RolDto> CreateRolAsync(RolDto RolDto)
        {
            try
            {
                ValidateRol(RolDto);

                var rol = new Rol
                {
                    Name = RolDto.Name,
                    Description = RolDto.Description // Si existe en la entidad
                };

                var rolCreado = await _rolData.CreateAsync(rol);

                return new RolDto
                {

                    Id = rolCreado.Id,


                    Name = rolCreado.Name,
                    Description = rolCreado.Description // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", RolDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRol(RolDto RolDto)
        {
            if (RolDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(RolDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }// Método para mapear de Rol a RolDTO
        private RolDto MapToDTO(Rol rol)
        {
            return new RolDto
            {
                Id = rol.Id,
                Name = rol.Name,
                Description = rol.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private Rol MapToEntity(RolDto rolDto)
        {
            return new Rol
            {
                Id = rolDto.Id,
                Name = rolDto.Name,
                Description = rolDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<RolDto> MapToDTOList(IEnumerable<Rol> roles)
        {
            var rolesDto = new List<RolDto>();
            foreach (var rol in roles)
            {
                rolesDto.Add(MapToDTO(rol));
            }
            return rolesDto;
        }

        public async Task DeleteLogicalRolAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRolAsync(RolDto rolDto)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateRol(RolDto rolDto)
        {
            throw new NotImplementedException();
        }
    }
}
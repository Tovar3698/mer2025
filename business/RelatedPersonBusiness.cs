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
    public class RelatedPersonBusiness
    {
        private readonly RelatedPersonData _relatedpersonData;
        private readonly ILogger<RelatedPersonBusiness> _logger;

        public RelatedPersonBusiness(RelatedPersonData relatedpersonData, ILogger<RelatedPersonBusiness> logger)
        {
            _relatedpersonData = relatedpersonData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<RelatedPersonDto>> GetAllRolesAsync()
        {
            try
            {
                var relatedpersons = await _relatedpersonData.GetAllAsync();
                var relatedpersonsDto = new List<RelatedPersonDto>();

                foreach (var relatedperson in relatedpersons)
                {
                    relatedpersonsDto.Add(new RelatedPersonDto
                    {
                        Id = relatedperson.Id,
                        Name = relatedperson.Name,
                        Description = relatedperson.Description // Si existe en la entidad
                    });
                }

                return relatedpersonsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<RelatedPersonDto> GetRelatedPersonByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var relatedperson = await _relatedpersonData.GetByIdAsync(id);
                if (relatedperson == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new RelatedPersonDto
                {
                    Id = relatedperson.Id,
                    Name = relatedperson.Name,
                    Description = relatedperson.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<RelatedPersonDto> CreateRolAsync(RelatedPersonDto RelatedPersonDto)
        {
            try
            {
                ValidateRelatedPerson(RelatedPersonDto);

                var relatedPerson = new RelatedPerson
                {
                    Id = RelatedPersonDto.Id,
                    Description = RelatedPersonDto.Description // Si existe en la entidad
                };

                var relatedpersonCreado = await _relatedpersonData.CreateAsync(relatedPerson);

                return new RelatedPersonDto
                {

                    Id = relatedpersonCreado.Id,
                    Name = relatedpersonCreado.Name,
                    Description = relatedpersonCreado.Description // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", RelatedPersonDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRelatedPerson(RelatedPersonDto RelatedPersonDto)
        {
            if (RelatedPersonDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(RelatedPersonDto.Id))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }// Método para mapear de Rol a RolDTO
        private RelatedPersonDto MapToDTO(RelatedPerson relatedperson)
        {
            return new RelatedPersonDto
            {
                Id = relatedperson.Id,
                Name = relatedperson.Name,
                Description = relatedperson.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private RelatedPerson MapToEntity(RelatedPersonDto relatedpersonDto)
        {
            return new RelatedPerson
            {
                Id = relatedpersonDto.Id,
                Name = relatedpersonDto.Name,
                Description = relatedpersonDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<RelatedPersonDto> MapToDTOList(IEnumerable<RelatedPerson> relatedpersons)
        {
            var relatedpersonsDto = new List<RelatedPersonDto>();
            foreach (var relatedperson in relatedpersons)
            {
                relatedpersonsDto.Add(MapToDTO(relatedperson));
            }
            return relatedpersonsDto;
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
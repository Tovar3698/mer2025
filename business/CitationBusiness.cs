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
    public class CitationBusiness
    {
        private readonly CitationData _citationData;
        private readonly ILogger<CitationBusiness> _logger;

        public CitationBusiness(CitationData citationData, ILogger<CitationBusiness> logger)
        {
            _citationData = citationData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<CitationDto>> GetAllCitationAsync()
        {
            try
            {
                var citation = await _citationData.GetAllAsync();
                var citationDto = MapToDTOList(citation);
                return citationDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<CitationDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var citation = await _citationData.GetByIdAsync(id);
                if (citation == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new CitationDto
                {
                    Id = citation.Id,
                    Name = citation.Name,
                    Description = citation.Description

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<CitationDto> CreatePermissionAsync(CitationDto CitationDto)
        {
            try
            {
                ValidateCitation(CitationDto);

                var citation = new Citation
                {
                    Name = CitationDto.Name,

                };

                var citationCreado = await _citationData.CreateAsync(citation);

                return new CitationDto
                {
                    Id = citationCreado.Id,
                    Name = citationCreado.Name,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", CitationDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateCitation(CitationDto CitationDto)
        {
            if (CitationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(CitationDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }        // Método para mapear de Rol a RolDTO
        private CitationDto MapToDTO(Citation Citation)
        {
            return new CitationDto
            {
                Id = Citation.Id,
                Name = Citation.Name,
                Description = Citation.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private Citation MapToEntity(CitationDto citationDto)
        {
            return new Citation
            {
                Id = citationDto.Id,
                Name = citationDto.Name,
                Description = citationDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<CitationDto> MapToDTOList(IEnumerable<Citation> citations)
        {
            var CitationDto = new List<CitationDto>();
            foreach (var citation in citations)
            {
                CitationDto.Add(MapToDTO(citation));
            }
            return CitationDto;
        }
    }
}

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
    public class TypeCitationBusiness
    {
        private readonly TypeCitationData _typecitationData;
        private readonly ILogger<TypeCitationBusiness> _logger;

        public TypeCitationBusiness(TypeCitationData typecitationData, ILogger<TypeCitationBusiness> logger)
        {
            _typecitationData = typecitationData;
            _logger = logger;
        }


        public async Task<IEnumerable<TypeCitationDto>> GetAllTypeCitationAsync()
        {
            try
            {
                var typescitation = await _typecitationData.GetAllAsync();
                var typecitationDto = new List<TypeCitationDto>();

                foreach (var typecitation in typescitation)
                {
                    typecitationDto.Add(new TypeCitationDto
                    {
                        Id = typecitation.Id,
                        Name = typecitation.Name,
                        Description = typecitation.Description
                    });
                }
                return typecitationDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        public async Task<TypeCitationDto> GetTypeCitationByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un usuario con ID inválido: {UsuarioId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var typecitation = await _typecitationData.GetByIdAsync(id);
                if (typecitation == null)
                {
                    _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                    throw new EntityNotFoundException("Usuario", id);
                }

                return new TypeCitationDto
                {
                    Id = typecitation.Id,
                    Name = typecitation.Name,
                    Description = typecitation.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }

        public async Task<TypeCitationDto> CreateTypeCitationAsync(TypeCitationDto TypeCitationDto)
        {
            try
            {
                ValidateTypeCitation(TypeCitationDto);

                var typescitation = new TypeCitation
                {
                    Id = TypeCitationDto.Id,
                    Name = TypeCitationDto.Name,
                    Description = TypeCitationDto.Description
                };

                var TypeCitationCreado = await _typecitationData.CreateAsync(typescitation);

                return new TypeCitationDto
                {
                    Id = TypeCitationCreado.Id,
                    Name = TypeCitationCreado.Name,
                    Description = TypeCitationCreado.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioId}", TypeCitationDto?.Id ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        private void ValidateTypeCitation(TypeCitationDto TypeCitationDto)
        {
            if (TypeCitationDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }
            if (TypeCitationDto.Id <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con UserId inválido");
                throw new Utilities.Exceptions.ValidationException("UserId", "El UserId es obligatorio y debe ser mayor que cero");
            }
        }

        public void GetAllTypeCitationPermission()
        {
            throw new NotImplementedException();
        }      // Método para mapear de Rol a RolDTO
        private TypeCitationDto MapToDTO(TypeCitation TypeCitation)
        {
            return new TypeCitationDto
            {
                Id = TypeCitation.Id,
                Name = TypeCitation.Name,
                Description = TypeCitation.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private TypeCitation MapToEntity(TypeCitationDto typecitationDto)
        {
            return new TypeCitation
            {
                Id = typecitationDto.Id,
                Name = typecitationDto.Name,
                Description = typecitationDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<TypeCitationDto> MapToDTOList(IEnumerable<TypeCitation> typecitations)
        {
            var TypeCitationDto = new List<TypeCitationDto>();
            foreach (var typecitation in typecitations)
            {
                TypeCitationDto.Add(MapToDTO(typecitation));
            }
            return TypeCitationDto;
        }
    }
}
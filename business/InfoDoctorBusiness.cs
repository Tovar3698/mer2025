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
    public class InfoDoctorBusiness
    {
        private readonly InfoDoctorData _infodoctorData;
        private readonly ILogger<InfoDoctorBusiness> _logger;

        public InfoDoctorBusiness(InfoDoctorData infodoctorData, ILogger<InfoDoctorBusiness> logger)
        {
            _infodoctorData = infodoctorData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<InfoDoctorDto>> GetAllInfoDoctorAsync()
        {
            try
            {
                var infodoctor = await _infodoctorData.GetAllAsync();
                var infodoctorDto = MapToDTOList(infodoctor);
                return infodoctorDto;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<InfoDoctorDto> GetPermissionByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var infodoctor = await _infodoctorData.GetByIdAsync(id);
                if (infodoctor == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new InfoDoctorDto
                {
                    Id = infodoctor.Id,
                    Specialty = infodoctor.Specialty,
                    Registration = infodoctor.Registration,
                    TableName = infodoctor.TableName

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }
        // Método para crear un rol desde un DTO
        public async Task<InfoDoctorDto> CreatePermissionAsync(InfoDoctorDto InfoDoctorDto)
        {
            try
            {
                ValidateInfoDoctor(InfoDoctorDto);

                var infodoctor = new InfoDoctor
                {
                    Id = InfoDoctorDto.Id,
                    Specialty = InfoDoctorDto.Specialty,
                    Registration = InfoDoctorDto.Registration,
                    TableName = InfoDoctorDto.TableName

                };

                var InfoDoctorCreado = await _infodoctorData.CreateAsync(infodoctor);

                return new InfoDoctorDto
                {
                    Id = InfoDoctorDto.Id,
                    Specialty = InfoDoctorDto.Specialty,
                    Registration = InfoDoctorDto.Registration,
                    TableName = InfoDoctorDto.TableName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", InfoDoctorDto?.Id);
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateInfoDoctor(InfoDoctorDto InfoDoctorDto)
        {
            if (InfoDoctorDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(InfoDoctorDto.TableName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }        // Método para mapear de Rol a RolDTO
        private InfoDoctorDto MapToDTO(InfoDoctor InfoDoctor)
        {
            return new InfoDoctorDto
            {
                Id = InfoDoctor.Id,
                Specialty = InfoDoctor.Specialty,
                Registration = InfoDoctor.Registration,
                TableName = InfoDoctor.TableName // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private InfoDoctor MapToEntity(InfoDoctorDto infodoctorDto)
        {
            return new InfoDoctor
            {
                Id = infodoctorDto.Id,
                Specialty = infodoctorDto.Specialty,
                Registration = infodoctorDto.Registration,
                TableName = infodoctorDto.TableName // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<InfoDoctorDto> MapToDTOList(IEnumerable<InfoDoctor> infodoctors)
        {
            var InfoDoctorDto = new List<InfoDoctorDto>();
            foreach (var infodoctor in infodoctors)
            {
                InfoDoctorDto.Add(MapToDTO(infodoctor));
            }
            return InfoDoctorDto;
        }
    }
}
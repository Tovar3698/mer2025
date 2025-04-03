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
    public class ModuleFormBusiness
    {
        private readonly ModuleFormData _moduleformData;
        private readonly ILogger<ModuleFormBusiness> _logger;

        public ModuleFormBusiness(ModuleFormData moduleformData, ILogger<ModuleFormBusiness> logger)
        {
            _moduleformData = moduleformData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<ModuleFormDto>> GetAllModulesAsync()
        {
            try
            {
                var moduldorms = await _moduleformData.GetAllAsync();
                var moduleformsDTO = new List<ModuleFormDto>();

                foreach (var moduleform in moduldorms)
                {
                    moduleformsDTO.Add(new ModuleFormDto
                    {
                        Id = moduleform.Id,
                        Name = moduleform.Name,
                        Description = moduleform.Description // Si existe en la entidad
                    });
                }

                return moduleformsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<ModuleFormDto> GetRolByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {RolId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var moduleform = await _moduleformData.GetByIdAsync(id);
                if (moduleform == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("Rol", id);
                }

                return new ModuleFormDto
                {
                    Id = moduleform.Id,
                    Name = moduleform.Name,
                    Description = moduleform.Description
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {RolId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        // Método para crear un rol desde un DTO
        public async Task<ModuleFormDto> CreateModuleFormAsync(ModuleFormDto ModuleFormDto)
        {
            try
            {
                ValidateModuleForm(ModuleFormDto);

                var moduleform = new ModuleForm
                {
                    Name = ModuleFormDto.Name,
                    Description = ModuleFormDto.Description // Si existe en la entidad
                };

                var moduleformCreado = await _moduleformData.CreateAsync(moduleform);

                return new ModuleFormDto
                {
                    Id = moduleformCreado.Id,
                    Name = moduleformCreado.Name,
                    Description = moduleformCreado.Description // Si existe en la entidad
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", ModuleFormDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el rol", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateModuleForm(ModuleFormDto ModuleFormDto)
        {
            if (ModuleFormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(ModuleFormDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private ModuleFormDto MapToDTO(ModuleForm moduleForm)
        {
            return new ModuleFormDto
            {
                Id = moduleForm.Id,
                Name = moduleForm.Name,
                Description = moduleForm.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private ModuleForm MapToEntity(ModuleFormDto moduleformDto)
        {
            return new ModuleForm
            {
                Id = moduleformDto.Id,
                Name = moduleformDto.Name,
                Description = moduleformDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<ModuleFormDto> MapToDTOList(IEnumerable<ModuleForm> moduleforms)
        {
            var moduleformsDto = new List<ModuleFormDto>();
            foreach (var moduleForm in moduleforms)
            {
                moduleformsDto.Add(MapToDTO(moduleForm));
            }
            return moduleformsDto;
        }

        public async Task GetAllModulesAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
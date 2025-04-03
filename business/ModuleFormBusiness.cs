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
        private readonly ILogger _logger;

        public ModuleFormBusiness(ModuleFormData moduleFormData, ILogger logger)
        {
            _moduleformData = moduleFormData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<ModuleFormDto>> GetAllModuleFormAsync()
        {
            try
            {
                var moduleForm = await _moduleformData.GetAllAsync();
                var moduleFormDTO = new List<ModuleFormDto>();

                foreach (var moduleform in moduleForm)
                {
                    moduleFormDTO.Add(new ModuleFormDto
                    {
                        ModuleFormId = moduleform.Id,
                        ModuleFormId = moduleform.Name,

                    });
                }

                return moduleFormDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los roles");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de roles", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<ModuleDto> GetModuleFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un rol con ID inválido: {ModuleId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del rol debe ser mayor que cero");
            }

            try
            {
                var moduleform = await _moduleformData.GetByIdAsync(id);
                if (moduleform == null)
                {
                    _logger.LogInformation("No se encontró ningún rol con ID: {RolId}", id);
                    throw new EntityNotFoundException("ModuleForm", id);
                }

                return new ModuleFormDto
                {
                    ModuleFormId = moduleform.Id,
                    ModuleFormName = moduleform.name,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID: {ModuleId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el rol con ID {id}", ex);
            }
        }

        public ModuleForm GetModuleForm()
        {
            return ModuleForm;
        }

        // Método para crear un rol desde un DTO
        public async Task<ModuleFormDto> CreateModuleFormAsync(ModuleFormDto ModuleFormDto, ModuleForm moduleForm)
        {
            try
            {
                ValidateModuleForm(ModuleFormDto);

                var module = new Rol
                {
                    Name = ModuleFormDto.ModuleFormName,

                };

                var moduleformCreado = await _moduleformData.CreateAsync(moduleForm);

                return new ModuleFormDto
                {
                    ModuleFormId = moduleformCreado.Id,
                    ModuleFormId = moduleformCreado.Name,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo rol: {RolNombre}", ModuleFormDto?.ModuleFormName ?? "null");
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

            if (string.IsNullOrWhiteSpace(ModuleFormDto.ModuleFormName))
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del rol es obligatorio");
            }
        }
    }
}
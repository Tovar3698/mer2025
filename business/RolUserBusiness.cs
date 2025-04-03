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
    public class RolUserBusiness
    {
        private readonly RolUserData _rolUserData;
        private readonly ILogger _logger;

        public RolUserBusiness(RolUserData rolUserData, ILogger logger)
        {
            _rolUserData = rolUserData;
            _logger = logger;
        }

        public async Task<IEnumerable<RolUserDto>> GetAllRolUserAsync()
        {
            try
            {
                var rolsuser = await _rolUserData.GetAllAsync();
                var roluserDto = new List<RolUserDto>();

                foreach (var roluser in rolsuser)
                {
                    roluserDto.Add(new RolUserDto
                    {
                        RolUserId  = roluser.RolId,
                        RolId = roluser.RolId,
                        UserId = roluser.RolId
                    });
                }
                return roluserDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        public async Task<RolUserDto> GetUserByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un usuario con ID inválido: {UsuarioId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var roluser = await _rolUserData.GetByIdAsync(id);
                if (roluser == null)
                {
                    _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                    throw new EntityNotFoundException("Usuario", id);
                }

                return new RolUserDto
                {
                    RolUserId = roluser.RolUserId,
                    RolId = roluser.RolId,
                    UserId = roluser.UserId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }

        public async Task<RolUserDto> CreateRolUserAsync(RolUserDto RolUserDto)
        {
            try
            {
                ValidateRolUser(RolUserDto);

                var rolsuser = new RolUser
                {
                    RolUserId = RolUserDto.RolId,
                    RolId = RolUserDto.RolId,
                    UserId = RolUserDto.UserId
                };

                var RolUserCreado = await _rolUserData.CreateAsync(rolsuser);

                return new RolUserDto
                {
                    RolUserId = RolUserCreado.RolUserId,
                    RolId = RolUserCreado.RolId,
                    UserId = RolUserCreado.UserId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioId}", RolUserDto?.UserId ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        private void ValidateRolUser(RolUserDto RolUserDto)
        {
            if (RolUserDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }
            if (RolUserDto.UserId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con UserId inválido");
                throw new Utilities.Exceptions.ValidationException("UserId", "El UserId es obligatorio y debe ser mayor que cero");
            }
        }

        public void GetAllRolUserPermission()
        {
            throw new NotImplementedException();
        }
    }
}

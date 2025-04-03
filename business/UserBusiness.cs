
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
    public class UserBusiness
    {
        private readonly UserData _userData;
        private readonly ILogger _logger;

        public UserBusiness(UserData userData, ILogger logger)
        {
            _userData = userData;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllUser()
        {
            try
            {
                var user = await _userData.GetAllAsync();
                var userDto = new List<UserDto>();

                foreach (var User in user)
                {
                    userDto.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name
                    });
                }
                return userDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos lo usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            if (id < 0)
            {

                _logger.LogWarning("Se intento obtener un usuario con ID invalido: {UsuarioId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var user = await _userData.GetByIdAsync(id);
                if (user == null)
                {
                    _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                    throw new EntityNotFoundException("Usuario", id);
                }

                return new UserDto
                {
                    Id = user.Id,
                    Name = user.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }
        public async Task<UserDto> CreateUserAsync(UserDto UserDto)
        {
            try
            {
                ValidateUser(UserDto, UserDto);
                var user = new  User 
                {
                    Name = UserDto.Name
                };
                var UserCreado = await _userData.CreateAsync(user);
                return new UserDto
                {
                    Id = UserCreado.Id,
                    Name = UserCreado.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioNombre}", UserDto?.UserName ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        private void ValidateUser(UserDto userDto1, UserDto userDto2)
        {
            throw new NotImplementedException();
        }

        private void ValidateUser(RolUserDto RolUserDto, UserDto userDto)
        {
            if (userDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }
            if (UserDto.UserId <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con UserId inválido");
                throw new Utilities.Exceptions.ValidationException("UserId", "El UserId es obligatorio y debe ser mayor que cero");
            }
        }
        public async Task GetUsersByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void GetAllUserAsync()
        {
            throw new NotImplementedException();
        }
    }
}

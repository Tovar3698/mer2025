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
    public class ScheduleBusiness
    {
        private readonly ScheduleData _scheduleData;
        private readonly ILogger<ScheduleBusiness> _logger;

        public ScheduleBusiness(ScheduleData scheduleData, ILogger<ScheduleBusiness> logger)
        {
            _scheduleData = scheduleData;
            _logger = logger;
        }

        public async Task<IEnumerable<ScheduleDto>> GetAllScheduleAsync()
        {
            try
            {
                var schedules = await _scheduleData.GetAllAsync();
                var scheduleDto = new List<ScheduleDto>();

                foreach (var schedule in schedules)
                {
                    scheduleDto.Add(new ScheduleDto
                    {
                        Id = schedule.Id,
                        InfoDoctorId = schedule.InfoDoctorId,
                        StartTime = schedule.StartTime
                    });
                }
                return scheduleDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los usuarios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de usuarios", ex);
            }
        }

        public async Task<ScheduleDto> GetScheduleByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un usuario con ID inválido: {UsuarioId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del usuario debe ser mayor que cero");
            }
            try
            {
                var schedule = await _scheduleData.GetByIdAsync(id);
                if (schedule == null)
                {
                    _logger.LogInformation("No se encontró ningún usuario con ID: {UserId}", id);
                    throw new EntityNotFoundException("Usuario", id);
                }

                return new ScheduleDto
                {
                    Id = schedule.Id,
                    InfoDoctor = schedule.InfoDoctor,
                    StartTime = schedule.StartTime
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el usuario con ID: {UserId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el usuario con ID {id}", ex);
            }
        }

        public async Task<ScheduleDto> CreateScheduleAsync(ScheduleDto scheduleDto)
        {
            try
            {
                ValidateSchedule(scheduleDto);

                var schedule = new Schedule
                {
                    Id = scheduleDto.Id,
                    InfoDoctorId = scheduleDto.InfoDoctorId,
                    StartTime = scheduleDto.StartTime
                };

                var scheduleCreado = await _scheduleData.CreateAsync(schedule);

                return new ScheduleDto
                {
                    Id = scheduleCreado.Id,
                    InfoDoctor = scheduleCreado.InfoDoctor,
                    StartTime = scheduleCreado.StartTime
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo usuario: {UsuarioId}", scheduleDto?.Id ?? 0);
                throw new ExternalServiceException("Base de datos", "Error al crear el usuario", ex);
            }
        }

        private void ValidateSchedule(ScheduleDto scheduleDto)
        {
            if (scheduleDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto rol no puede ser nulo");
            }
            if (scheduleDto.Id <= 0)
            {
                _logger.LogWarning("Se intentó crear/actualizar un rol con UserId inválido");
                throw new Utilities.Exceptions.ValidationException("UserId", "El UserId es obligatorio y debe ser mayor que cero");
            }
        }

        public void GetAllSchedulePermission()
        {
            throw new NotImplementedException();
        }

        // Método para mapear de Rol a RolDTO
        private ScheduleDto MapToDTO(Schedule schedule)
        {
            return new ScheduleDto
            {
                Id = schedule.Id,
                InfoDoctor = schedule.InfoDoctor,
                StartTime = schedule.StartTime
            };
        }

        // Método para mapear de RolDTO a Rol
        private Schedule MapToEntity(ScheduleDto scheduleDto)
        {
            return new Schedule
            {
                Id = scheduleDto.Id,
                InfoDoctorId = scheduleDto.InfoDoctorId,
                StartTime = scheduleDto.StartTime
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<ScheduleDto> MapToDTOList(IEnumerable<Schedule> schedules)
        {
            var scheduleDto = new List<ScheduleDto>();
            foreach (var schedule in schedules)
            {
                scheduleDto.Add(MapToDTO(schedule));
            }
            return scheduleDto;
        }
    }
}
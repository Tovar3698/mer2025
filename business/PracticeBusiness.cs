using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using Utilities.Exceptions;


namespace Business
{
    public class PracticeBusiness
    {
        private readonly PracticeData _practiceData;
        private readonly ILogger<PracticeBusiness> _logger;

        public PracticeBusiness(PracticeData practiceData, ILogger<PracticeBusiness> logger)
        {
            _practiceData = practiceData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PracticeDto>> GetAllPerson()
        {
            try
            {
                var practices = await _practiceData.GetAllAsync();
                var practiceDto = new List<PracticeDto>();

                foreach (var practice in practices)
                {
                    practiceDto.Add(new PracticeDto
                    {
                        Id = practice.Id,
                        Name = practice.Name,
                        Description = practice.Description,


                    });
                }

                return practiceDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PracticeDto> GetPersonByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var practice = await _practiceData.GetByIdAsync(id);
                if (practice == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }

                return new PracticeDto
                {
                    Id = practice.Id,
                    Name = practice.Name,
                    Description = practice.Description

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }

        // Método para crear un formulario desde un DTO
        public async Task<PracticeDto> CreatePracticeAsync(PracticeDto PracticeDto)
        {
            try
            {
                ValidatePractice(PracticeDto);

                var practice = new Practice
                {
                    Name = PracticeDto.Name,
                    Description = PracticeDto.Description // Se agregó la descripción
                };

                var personCreado = await _practiceData.CreateAsync(practice);

                return new PracticeDto
                {
                    Id = personCreado.Id,
                    Name = personCreado.Name,
                    Description = personCreado.Description // Se agregó la descripción
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", PracticeDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePractice(PracticeDto PracticeDto)
        {
            if (PracticeDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto formulario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PracticeDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formulario con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del formulario es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private PracticeDto MapToDTO(Practice Practice)
        {
            return new PracticeDto
            {
                Id = Practice.Id,
                Name = Practice.Name,
                Description = Practice.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private Practice MapToEntity(PracticeDto practiceDto)
        {
            return new Practice
            {
                Id = practiceDto.Id,
                Name = practiceDto.Name,
                Description = practiceDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<PracticeDto> MapToDTOList(IEnumerable<Practice> practices)
        {
            var PracticeDto = new List<PracticeDto>();
            foreach (var practice in practices)
            {
                PracticeDto.Add(MapToDTO(practice));
            }
            return PracticeDto;
        }
    }
}

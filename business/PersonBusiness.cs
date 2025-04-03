using Data;
using Entity.DTOs;
using Entity.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Exceptions;

namespace Business
{
    public class PersonBusiness
    {
        private readonly PersonData _personData;
        private readonly ILogger<PersonBusiness> _logger;

        public PersonBusiness(PersonData personData, ILogger<PersonBusiness> logger)
        {
            _personData = personData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<PersonDto>> GetAllPerson()
        {
            try
            {
                var persons = await _personData.GetAllAsync();
                var personDto = new List<PersonDto>();

                foreach (var person in persons)
                {
                    personDto.Add(new PersonDto
                    {
                        Id = person.Id,
                        Name = person.Name,
                        Description = person.Description,


                    });
                }

                return personDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<PersonDto> GetPersonByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var person = await _personData.GetByIdAsync(id);
                if (person == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }

                return new PersonDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    Description = person.Description

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }

        // Método para crear un formulario desde un DTO
        public async Task<PersonDto> CreatePersonAsync(PersonDto PersonDto)
        {
            try
            {
                ValidatePerson(PersonDto);

                var person = new Person
                {
                    Name = PersonDto.Name,
                    Description = PersonDto.Description // Se agregó la descripción
                };

                var personCreado = await _personData.CreateAsync(person);

                return new PersonDto
                {
                    Id = personCreado.Id,
                    Name = personCreado.Name,
                    Description = personCreado.Description // Se agregó la descripción
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", PersonDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidatePerson(PersonDto PersonDto)
        {
            if (PersonDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto formulario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(PersonDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formulario con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del formulario es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private PersonDto MapToDTO(Person Person)
        {
            return new PersonDto
            {
                Id = Person.Id,
                Name = Person.Name,
                Description = Person.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private Person MapToEntity(PersonDto personDto)
        {
            return new Person
            {
                Id = personDto.Id,
                Name = personDto.Name,
                Description = personDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<PersonDto> MapToDTOList(IEnumerable<Person> persons)
        {
            var PersonDto = new List<PersonDto>();
            foreach (var person in persons)
            {
                PersonDto.Add(MapToDTO(person));
            }
            return PersonDto;
        }
    }
}

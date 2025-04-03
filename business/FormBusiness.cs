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
    public class FormBusiness
    {
        private readonly FormData _formData;
        private readonly ILogger _logger;

        public FormBusiness(FormData formData, ILogger logger)
        {
            _formData = formData;
            _logger = logger;
        }

        // Método para obtener todos los roles como DTOs
        public async Task<IEnumerable<FormDto>> GetAllForm()
        {
            try
            {
                var forms = await _formData.GetAllAsync();
                var formsDto = new List<FormDto>();

                foreach (var form in forms)
                {
                    formsDto.Add(new FormDto
                    {
                        Id = form.Id,
                        Name = form.Name,
                        Description = form.Description,
                        

                    });
                }

                return formsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<FormDto> GetFormByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Se intentó obtener un formulario con ID inválido: {FormId}", id);
                throw new Utilities.Exceptions.ValidationException("id", "El ID del formulario debe ser mayor que cero");
            }

            try
            {
                var form = await _formData.GetByIdAsync(id);
                if (form == null)
                {
                    _logger.LogInformation("No se encontró ningún formulario con ID: {FormId}", id);
                    throw new EntityNotFoundException("Formulario", id);
                }

                return new FormDto
                {
                    Id = form.Id,
                    Name = form.Name,
                    Description = form.Description

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }

        // Método para crear un formulario desde un DTO
        public async Task<FormDto> CreateFormAsync(FormDto FormDto)
        {
            try
            {
                ValidateForm(FormDto);

                var form = new Form
                {
                    Name = FormDto.Name,
                    Description = FormDto.Description // Se agregó la descripción
                };

                var formCreado = await _formData.CreateAsync(form);

                return new FormDto
                {
                    Id = formCreado.Id,
                    Name = formCreado.Name,
                    Description = formCreado.Description // Se agregó la descripción
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", FormDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateForm(FormDto FormDto)
        {
            if (FormDto == null)
            {
                throw new Utilities.Exceptions.ValidationException("El objeto formulario no puede ser nulo");
            }

            if (string.IsNullOrWhiteSpace(FormDto.Name))
            {
                _logger.LogWarning("Se intentó crear/actualizar un formulario con Name vacío");
                throw new Utilities.Exceptions.ValidationException("Name", "El Name del formulario es obligatorio");
            }
        }

        // Método para mapear de Rol a RolDTO
        private FormDto MapToDTO(Form Form)
        {
            return new FormDto
            {
                Id = Form.Id,
                Name = Form.Name,
                Description = Form.Description // Si existe en la entidad
            };
        }

        // Método para mapear de RolDTO a Rol
        private Form MapToEntity(FormDto formDto)
        {
            return new Form
            {
                Id = formDto.Id,
                Name = formDto.Name,
                Description = formDto.Description // Si existe en la entidad
            };
        }

        // Método para mapear una lista de Rol a una lista de RolDTO
        private IEnumerable<FormDto> MapToDTOList(IEnumerable<Form> forms)
        {
            var FormDto = new List<FormDto>();
            foreach (var form in forms)
            {
                FormDto.Add(MapToDTO(form));
            }
            return FormDto;
        }
    }
}

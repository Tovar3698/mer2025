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
        public async Task<IEnumerable<RolUserDto>> GetAllForm()
        {
            try
            {
                var forms = await _formData.GetAllAsync();
                var formsDTO = new List<RolUserDto>();

                foreach (var form in forms)
                {
                    formsDTO.Add(new RolUserDto
                    {
                        Id = form.Id,
                        Name = form.Name,
                        Form = form.Description

                    });
                }

                return formsDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todos los formularios");
                throw new ExternalServiceException("Base de datos", "Error al recuperar la lista de formularios", ex);
            }
        }

        // Método para obtener un rol por ID como DTO
        public async Task<RolUserDto> GetFormByIdAsync(int id)
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

                return new RolUserDto
                {
                    Id = form.Id,
                    Name = form.Name,
                    Form = form.Description

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID: {FormId}", id);
                throw new ExternalServiceException("Base de datos", $"Error al recuperar el formulario con ID {id}", ex);
            }
        }

        // Método para crear un formulario desde un DTO
        public async Task<RolUserDto> CreateFormAsync(RolUserDto FormDto)
        {
            try
            {
                ValidateRol(FormDto);

                var form = new Form
                {
                    Name = FormDto.Name,
                    Description = FormDto.Form // Se agregó la descripción
                };

                var formCreado = await _formData.CreateAsync(form);

                return new RolUserDto
                {
                    Id = formCreado.Id,
                    Name = formCreado.Name,
                    Form = formCreado.Description // Se agregó la descripción
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear nuevo formulario: {FormName}", FormDto?.Name ?? "null");
                throw new ExternalServiceException("Base de datos", "Error al crear el formulario", ex);
            }
        }

        // Método para validar el DTO
        private void ValidateRol(RolUserDto FormDto)
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

        public void GetAllFormAsync()
        {
            throw new NotImplementedException();
        }
    }
}

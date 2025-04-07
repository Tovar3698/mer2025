using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class CitationData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor que recibe recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Intancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>

        public CitationData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los formularios almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de formularios</returns>
        public async Task<IEnumerable<Citation>> GetAllAsync()
        {
            return await _context.Set<Citation>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un formulario especifico por su identificador
        /// </summary>
        public async Task<Citation?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Citation>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el formulario con ID {FormId}", id);
                throw; //Re-lanza la excepcion para sea manejada en capas superiores
            }
        }
        /// <summary>
        /// Crea un nuevo formulario en la base de datos
        /// </summary>
        /// <param name="form"></param>
        /// <returns>el formulario creado.</returns>
        /// 
        public async Task<Citation> CreateAsync(Citation citation)
        {
            try
            {
                await _context.Set<Citation>().AddAsync(citation);
                await _context.SaveChangesAsync();
                return citation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el formulario: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Actualiza un formulario existente en la base de datos
        /// </summary>
        /// <param name="form">Objeto con la informacion actualizada</param>
        /// <returns>True si la operacion fue exitosa, False en caso contrario</returns>
        public async Task<bool> UpdateAsync(Citation citation)
        {
            try
            {
                _context.Set<Citation>().Update(citation);
                await _context.SaveChagesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el formulario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var citation = await _context.Set<Citation>().FindAsync(id);
                if (citation == null)
                    return false;
                _context.Set<Citation>().Remove(citation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar formulario: {ex.Message}");
                return false;
            }
        }
    }
}

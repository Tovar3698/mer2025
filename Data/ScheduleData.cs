using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data
{
    /// <summary>
    /// Repositorio encargado de la gestión de la entidad Rol en la base de datos.
    /// </summary>
    public class ScheduleData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Instancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>
        public ScheduleData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de roles.</returns>
        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            return await _context.Set<Schedule>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un rol específico por su identificador.
        /// </summary>
        public async Task<Schedule?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Schedule>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol con ID {RolId}", id);
                throw;
            }
        }
        /// <summary>
        /// Crea un nuevo rol en la base de datos.
        /// </summary>
        /// <param name="rol">Instancia del rol a crear.</param>
        /// <returns>El rol creado.</returns>
        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            try
            {
                await _context.Set<Schedule>().AddAsync(schedule);
                await _context.SaveChangesAsync();
                return schedule;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Actualiza un rol existente en la base de datos.
        /// </summary>
        /// <param name="rol">Objeto con la información actualizada.</param>
        /// <returns>True si la operación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> UpdateAsync(Schedule schedule)
        {
            try
            {
                _context.Set<Schedule>().Update(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// Elimina un rol de la base de datos.
        /// </summary>
        /// <param name="id">Identificador único del rol a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False en caso contrario.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var schedule = await _context.Set<Schedule>().FindAsync(id);
                if (schedule == null)
                    return false;

                _context.Set<Schedule>().Remove(schedule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el rol: {ex.Message}");
                return false;
            }
        }
    }
}

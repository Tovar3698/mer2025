using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    /// <summary>
    /// Repositorio encargado de la getion de la entidad Rol en la base de datos
    /// </summary>
    public class PracticeData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor que recibe recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Intancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>

        public PracticeData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de roles</returns>
        public async Task<IEnumerable<Practice>> GetAllAsync()
        {
            return await _context.Set<Practice>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un rol especifico por su identificador
        /// </summary>
        public async Task<Practice?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<Practice>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener rol con ID {PersonId}", id);
                throw; //Re-lanza la excepcion para sea manejada en capas superiores
            }
        }
        /// <summary>
        /// Crea un nuevo rol en la base de datos
        /// </summary>
        /// <param name="person"></param>
        /// <returns>el rol creado.</returns>
        /// 
        public async Task<Practice> CreateAsync(Practice practice)
        {
            try
            {
                await _context.Set<Practice>().AddAsync(practice);
                await _context.SaveChangesAsync();
                return practice;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el rol: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Actualiza un rol existente en la base de datos
        /// </summary>
        /// <param name="person">Objeto con la informacion actualizada</param>
        /// <returns>True si la operacion fue exitosa, False en caso contrario</returns>
        public async Task<bool> UpdateAsync(Practice practice)
        {
            try
            {
                _context.Set<Practice>().Update(practice);
                await _context.SaveChagesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el rol: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var practice = await _context.Set<Practice>().FindAsync(id);
                if (practice == null)
                    return false;
                _context.Set<Practice>().Remove(practice);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar rol: {ex.Message}");
                return false;
            }
        }
    }
}

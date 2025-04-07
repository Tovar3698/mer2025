using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    /// <summary>
    /// Repositorio encargado de la getion de la entidad Rol en la base de datos
    /// </summary>
    public class RelatedPersonData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor que recibe recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Intancia de <see cref="ApplicationDbContext"/> para la conexión con la base de datos.</param>

        public RelatedPersonData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todos los roles almacenados en la base de datos.
        /// </summary>
        /// <returns>Lista de roles</returns>
        public async Task<IEnumerable<RelatedPerson>> GetAllAsync()
        {
            return await _context.Set<RelatedPerson>().ToListAsync();
        }

        /// <summary>
        /// Obtiene un rol especifico por su identificador
        /// </summary>
        public async Task<RelatedPerson?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<RelatedPerson>().FindAsync(id);
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
        public async Task<RelatedPerson> CreateAsync(RelatedPerson relatedperson)
        {
            try
            {
                await _context.Set<RelatedPerson>().AddAsync(relatedperson);
                await _context.SaveChangesAsync();
                return relatedperson;
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
        public async Task<bool> UpdateAsync(RelatedPerson relatedperson)
        {
            try
            {
                _context.Set<RelatedPerson>().Update(relatedperson);
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
                var relatedperson = await _context.Set<RelatedPerson>().FindAsync(id);
                if (relatedperson == null)
                    return false;
                _context.Set<RelatedPerson>().Remove(relatedperson);
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

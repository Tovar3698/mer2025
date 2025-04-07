using Entity.Contexts;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Data
{
    public class TypeCitationData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TypeCitationData> _logger;

        public TypeCitationData(ApplicationDbContext context, ILogger<TypeCitationData> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IEnumerable<TypeCitation>> GetAllAsync()
        {
            return await _context.Set<TypeCitation>().ToListAsync();
        }
        public async Task<TypeCitation?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<TypeCitation>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario con ID {UserId}", id);
                throw;
            }
        }
        public async Task<TypeCitation> CreateAsync(TypeCitation typeCitation)
        {
            try
            {
                await _context.Set<TypeCitation>().AddAsync(typeCitation);
                await _context.SaveChangesAsync();
                return typeCitation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro al crear el usuario: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(TypeCitation typecitation)
        {
            try
            {
                _context.Set<TypeCitation>().Update(typecitation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el usuario: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var typecitation = await _context.Set<TypeCitation>().FindAsync(id);
                if (typecitation == null)
                    return false;
                _context.Set<TypeCitation>().Remove(typecitation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el usuario: {ex.Message}");
                return false;
            }
        }
    }
}

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
    public class ModuleFormData
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ModuleFormData(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<ModuleForm>> GetAllAsync()
        {
            return await _context.Set<ModuleForm>().ToListAsync();
        }
        public async Task<ModuleForm?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Set<ModuleForm>().FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el Modulo con sus fomularios con ID {ModuleFormId}", id);
                throw;
            }
        }
        public async Task<ModuleForm> CreateAsync(ModuleForm moduleForm)
        {
            try
            {
                await _context.Set<ModuleForm>().AddAsync(moduleForm);
                await _context.SaveChangesAsync();
                return moduleForm;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al crear el Modulo con sus permisos: {ex.Message}");
                throw;
            }
        }
        public async Task<bool> UpdateAsync(ModuleForm moduleForm)
        {
            try
            {
                _context.Set<ModuleForm>().Update(moduleForm);
                await _context.SaveChagesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al actualizar el modulo con sus permisos: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var moduleForm = await _context.Set<ModuleForm>().FindAsync(id);
                if (moduleForm == null)
                    return false;

                _context.Set<ModuleForm>().Remove(moduleForm);
                await _context.SaveChangesAsync();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar el modulo con sus permisos { ex.Message}");
                return false;
            }
        }

        public void Create(object moduleform)
        {
            throw new NotImplementedException();
        }
    }
}

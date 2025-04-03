
using Microsoft.Extensions.Logging;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Entity.Contexts;

namespace Data
{
    public class RolData
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<RolData> _logger;
        private readonly string ConnectionString;
        ///<param name="conext">Instancia de <see cref="ApplicationDbContext"></param> para la conexion con la base de datos
        public RolData(ApplicationDbContext context, ILogger<RolData> logger, IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
            _logger = logger;
        }



        public async Task<IEnumerable<Rol>> GetAllAsync()

        {
            return await _context.Set<Rol>().ToListAsync();
        }
        public async Task<Rol?> GetbyIdAsync(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT Id, Name FROM Roles WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new Rol
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el rol con ID {RolId}", id);
            }

            return null;
        }

        ///<param name="rol"> Insancia del rol a crear</param>>


        public async Task<bool> CreateAsync(Rol rol)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO Roles (Name) VALUES (@Name)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", rol.Name);
                        int filasAfectadas = await command.ExecuteNonQueryAsync();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el rol.");
                return false;
            }
        }


        public async Task<bool> UpdateAsync(Rol rol)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = "UPDATE Roles SET Name = @Name WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", rol.Name);
                        command.Parameters.AddWithValue("@Id", rol.Id);

                        int filasAfectadas = await command.ExecuteNonQueryAsync();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el rol con ID {RolId}", rol.Id);
                return false;
            }
        }

        ///<param name="id">Identificador unico del rol a eliminar.</param>>


        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM Roles WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        int filasAfectadas = await command.ExecuteNonQueryAsync();
                        return filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el rol con ID {RolId}", id);
                return false;
            }
        }

    }
}
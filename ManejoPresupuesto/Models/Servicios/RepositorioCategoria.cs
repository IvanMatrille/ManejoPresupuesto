using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ManejoPresupuesto.Models.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
    }

    public class RepositorioCategoria :IRepositorioCategorias
    {
        private readonly string connectionString;

        public RepositorioCategoria(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        
        public async Task Crear(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"
                                    INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                                    Values (@Nombre, @TipoOperacionId, @UsuarioId);
                        
                                    SELECT SCOPE_IDENTITY();
                                    ", categoria);

            categoria.Id = id;
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                "SELECT *  FROM Categorias WHERE UsuarioId = @usuarioId", new {usuarioId} );
        }
    }
}

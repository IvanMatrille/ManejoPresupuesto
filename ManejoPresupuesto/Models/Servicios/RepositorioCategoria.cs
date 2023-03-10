using Dapper;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace ManejoPresupuesto.Models.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(Categoria categoria);
        Task Borrar(int id);
        Task Crear(Categoria categoria);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacion);
        Task<IEnumerable<Categoria>> Obtener(int usuarioId);
        Task<Categoria> ObtenerPorId(int id, int usuarioId);
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

        public async Task<Categoria> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Categoria>(
                        @"SELECT * FROM Categorias WHERE Id = @Id AND UsuarioId = @UsuarioId",
                        new { id, usuarioId });
        }

        public async Task Actualizar(Categoria categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categorias 
                    SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                    WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(
                @"DELETE Categorias WHERE Id = @Id", new { id });
        }

        public async Task<IEnumerable<Categoria>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Categoria>(
                @"SELECT *  FROM Categorias 
                WHERE UsuarioId = @usuarioId AND TipoOperacionId = @tipoOperacionId", new { usuarioId, tipoOperacionId });
        }
    }
}

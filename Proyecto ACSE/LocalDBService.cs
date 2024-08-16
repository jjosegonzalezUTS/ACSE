using SQLite;
using System.IO;
using System.Threading.Tasks;

namespace Proyecto_ACSE
{
    public class LocalDBService
    {
        private const string DB_NAME = "acse_db.db3";
        private readonly SQLiteAsyncConnection _connection;
        public UserService UserService { get; }
        public CaseService CaseService { get; }

        public LocalDBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<User>();
            _connection.CreateTableAsync<Case>();

            UserService = new UserService(_connection);
            CaseService = new CaseService(_connection);
        }

        async Task DeleteAllRecords()
        {
            await _connection.DeleteAllAsync<User>();
            await _connection.DeleteAllAsync<Case>();
        }

        async Task ResetDatabase()
        {
            await _connection.DropTableAsync<User>();
            await _connection.DropTableAsync<Case>();

            await _connection.CreateTableAsync<User>();
            await _connection.CreateTableAsync<Case>();
        }
    }
}
using SQLite;

namespace Proyecto_ACSE
{
    public class UserService
    {
        private readonly SQLiteAsyncConnection _connection;

        public UserService(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _connection.Table<User>().ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _connection.Table<User>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _connection.Table<User>().Where(x => x.Name == name).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _connection.Table<User>().Where(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            return await _connection.Table<User>().Where(x => x.Email == email && x.Password == password).FirstOrDefaultAsync();
        }

        public async Task AddUser(User user)
        {
            await _connection.InsertAsync(user);
        }

        public async Task UpdateUser(User user)
        {
            await _connection.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            await _connection.Table<Case>().DeleteAsync(c => c.UserId == userId);
            await _connection.Table<User>().DeleteAsync(u => u.Id == userId);
        }

        public async Task EnsureAdminUserExists()
        {
            var adminUser = await _connection.Table<User>().Where(x => x.Name == "admin").FirstOrDefaultAsync();
            if (adminUser == null)
            {
                var admin = new User
                {
                    Name = "admin",
                    Email = "admin",
                    Password = "admin",
                    Role = User.UserRole.Admin
                };
                await AddUser(admin);
            }
        }

        public async Task EnsureAnonymousUserExists()
        {
            var anonymousUser = await _connection.Table<User>().Where(x => x.Name == "AnonymousReports").FirstOrDefaultAsync();
            if (anonymousUser == null)
            {
                var anonymous = new User
                {
                    Name = "AnonymousReports",
                    Email = "AnonymousReports",
                    Password = "AnonymousReports",
                    Role = User.UserRole.User
                };
                await AddUser(anonymous);
            }
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            var user = await _connection.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
            return user != null;
        }
    }
}
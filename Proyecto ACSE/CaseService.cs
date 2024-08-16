using SQLite;

namespace Proyecto_ACSE
{
    public class CaseService
    {
        private readonly SQLiteAsyncConnection _connection;

        public CaseService(SQLiteAsyncConnection connection)
        {
            _connection = connection;
        }

        public async Task AddCase(Case newCase)
        {
            await _connection.InsertAsync(newCase);
        }

        public async Task UpdateCase(Case updatedCase)
        {
            await _connection.UpdateAsync(updatedCase);
        }

        public Task<int> DeleteCaseAsync(int caseId)
        {
            return _connection.Table<Case>().DeleteAsync(c => c.Id == caseId);
        }

        public async Task<List<Case>> GetCasesByUserId(int userId)
        {
            return await _connection.Table<Case>().Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task<Case> GetCaseById(int caseId)
        {
            return await _connection.Table<Case>().Where(c => c.Id == caseId).FirstOrDefaultAsync();
        }

        public async Task<List<Case>> GetAllCases()
        {
            return await _connection.Table<Case>().ToListAsync();
        }
    }
}
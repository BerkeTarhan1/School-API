using MongoDB.Driver;
using SchoolAPI.Models;

namespace SchoolAPI.Services
{
	public class UserService
	{
		private readonly IMongoCollection<User> _users;

		public UserService(MongoDBSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			_users = database.GetCollection<User>(settings.UsersCollectionName ?? "Users");
		}

		public async Task<User?> GetUserByUsernameAsync(string username)
		{
			return await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
		}

		public async Task CreateUserAsync(User user)
		{
			await _users.InsertOneAsync(user);
		}

		public async Task<bool> UserExistsAsync(string username)
		{
			return await _users.Find(user => user.Username == username).AnyAsync();
		}
	}
}
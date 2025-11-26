using MongoDB.Driver;
using SchoolAPI.Models;

namespace SchoolAPI.Services
{
	public class StudentService
	{
		private readonly IMongoCollection<Student> _students;

		public StudentService(MongoDBSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);
			var database = client.GetDatabase(settings.DatabaseName);
			_students = database.GetCollection<Student>(settings.CollectionName);
		}

		public async Task<List<Student>> GetAsync() =>
			await _students.Find(student => true).ToListAsync();

		public async Task<Student?> GetAsync(string id) =>
			await _students.Find(student => student.Id == id).FirstOrDefaultAsync();

		public async Task CreateAsync(Student student) =>
			await _students.InsertOneAsync(student);

		public async Task UpdateAsync(string id, Student student) =>
			await _students.ReplaceOneAsync(student => student.Id == id, student);

		public async Task RemoveAsync(string id) =>
			await _students.DeleteOneAsync(student => student.Id == id);
	}
}

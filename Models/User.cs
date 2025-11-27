using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolAPI.Models
{
	public class User
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		[BsonElement("username")]
		public string Username { get; set; } = null!;

		[BsonElement("passwordHash")]
		public string PasswordHash { get; set; } = null!;

		[BsonElement("role")]
		public string Role { get; set; } = "User";

		[BsonElement("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
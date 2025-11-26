using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SchoolAPI.Models
{
	public class Student
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		[BsonElement("name")]
		public string Name { get; set; } = null!;

		[BsonElement("birthYear")]
		public int BirthYear { get; set; }

		[BsonElement("class")]
		public string Class { get; set; } = null!;

		[BsonElement("createdAt")]
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}

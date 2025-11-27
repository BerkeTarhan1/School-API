namespace SchoolAPI.Models
{
	public class MongoDBSettings
	{
		public string ConnectionString { get; set; } = null!;
		public string DatabaseName { get; set; } = null!;
		public string CollectionName { get; set; } = null!;
		public string UsersCollectionName { get; set; } = "Users"; // Add this line
	}
}
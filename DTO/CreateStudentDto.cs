namespace SchoolAPI.DTO
{
	public class CreateStudentDto
	{
		public string Name { get; set; } = null!;
		public int BirthYear { get; set; }
		public string Class { get; set; } = null!;
	}
}

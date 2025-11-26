using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTO;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StudentsController : ControllerBase
	{
		private readonly StudentService _studentService;

		public StudentsController(StudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpGet]
		public async Task<ActionResult<List<Student>>> Get()
		{
			var students = await _studentService.GetAsync();
			return Ok(students);
		}

		[HttpGet("{id:length(24)}")]
		public async Task<ActionResult<Student>> Get(string id)
		{
			var student = await _studentService.GetAsync(id);

			if (student is null)
			{
				return NotFound();
			}

			return Ok(student);
		}

		[HttpPost]
		public async Task<IActionResult> Post(CreateStudentDto createStudentDto)
		{
			if (string.IsNullOrWhiteSpace(createStudentDto.Name) ||
				createStudentDto.BirthYear < 1900 || createStudentDto.BirthYear > DateTime.Now.Year ||
				string.IsNullOrWhiteSpace(createStudentDto.Class))
			{
				return BadRequest("Invalid student data");
			}

			var student = new Student
			{
				Name = createStudentDto.Name,
				BirthYear = createStudentDto.BirthYear,
				Class = createStudentDto.Class
				// Id is not set - MongoDB will generate it automatically
			};

			await _studentService.CreateAsync(student);
			return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
		}

		[HttpPut("{id:length(24)}")]
		public async Task<IActionResult> Update(string id, Student updatedStudent)
		{
			var student = await _studentService.GetAsync(id);

			if (student is null)
			{
				return NotFound();
			}

			updatedStudent.Id = student.Id;
			await _studentService.UpdateAsync(id, updatedStudent);

			return NoContent();
		}

		[HttpDelete("{id:length(24)}")]
		public async Task<IActionResult> Delete(string id)
		{
			var student = await _studentService.GetAsync(id);

			if (student is null)
			{
				return NotFound();
			}

			await _studentService.RemoveAsync(id);
			return NoContent();
		}
	}
}

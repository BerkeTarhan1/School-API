using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolAPI.DTO;
using SchoolAPI.Models;
using SchoolAPI.Services;

namespace SchoolAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize] // Require authentication for all endpoints
	public class StudentsController : ControllerBase
	{
		private readonly StudentService _studentService;

		public StudentsController(StudentService studentService)
		{
			_studentService = studentService;
		}

		[HttpGet]
		[AllowAnonymous] // Allow viewing students without login
		public async Task<ActionResult<List<Student>>> Get()
		{
			var students = await _studentService.GetAsync();
			return Ok(students);
		}

		[HttpGet("{id:length(24)}")]
		[AllowAnonymous] // Allow viewing single student without login
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
		[Authorize(Roles = "Admin")] // Only Admins can create students
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
		[Authorize(Roles = "Admin")] // Only Admins can update students
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
		[Authorize(Roles = "Admin")] // Only Admins can delete students
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
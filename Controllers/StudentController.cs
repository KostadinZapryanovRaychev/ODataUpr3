using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using OdataSolution.Models;
using OdataSolution.Services;


[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    private string GenerateETagForStudent(Student student)
    {
        // Here you can generate an ETag based on the current state of the entity
        // For example, using a hash of the student's data or a timestamp
        return $"W/\"{student.Id}-{student.Name}-{student.Score}\"";  // Simple version-based ETag
    }

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [EnableQuery]
    public ActionResult<IQueryable<Student>> GetAllStudents()
    {
        var students = _studentService.GetAllStudents();
        return Ok(students);
    }

    [HttpGet("{id}")]
    [EnableQuery]
    [Authorize]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();
        Response.Headers.Add("ETag", student.ETag);

        return Ok(student);
    }

    [HttpPost]
    [EnableQuery]
    [Authorize]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        var created = await _studentService.CreateStudentAsync(student);
        Response.Headers.Add("ETag", created.ETag);

        return CreatedAtAction(nameof(GetStudentById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    [EnableQuery]
    [Authorize]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
    {
        var currentStudent = await _studentService.GetStudentByIdAsync(id);
        if (currentStudent == null)
            return NotFound();

        // For OData, you may want to support optimistic concurrency with ETag
        if (!HttpContext.Request.Headers.ContainsKey("If-Match"))
        {
            return BadRequest("Missing ETag header for concurrency check");
        }

        var etagHeader = HttpContext.Request.Headers["If-Match"].ToString();
        if (etagHeader != currentStudent.ETag)
        {
            return Conflict("The resource has been modified by another user.");
        }

        // Update the student and regenerate the ETag
        student.ETag = GenerateETagForStudent(student);

        var updatedStudent = await _studentService.UpdateStudentAsync(id, student);
        if (updatedStudent == null)
            return NotFound();

        // Set ETag header in response
        Response.Headers.Add("ETag", updatedStudent.ETag);

        return Ok(updatedStudent);
    }

    [HttpDelete("{id}")]
    [EnableQuery]
    [Authorize]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();

        var deleted = await _studentService.DeleteStudentAsync(id);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using OdataSolution.Models;
using OdataSolution.Services;

namespace OdataSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult <IQueryable<Student>> GetAllStudents()
        {
            IQueryable<Student> students = _studentService.GetAllStudents();
            return Ok(students);
        }
    }
}

// https://localhost:7109/api/Student
// https://localhost:7109/api/Student?$select=Name

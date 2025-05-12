using Microsoft.EntityFrameworkCore;
using OdataSolution.Data;
using OdataSolution.Models;

namespace OdataSolution.Services
{
    public class StudentService : IStudentService
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Generates an ETag based on student properties (ID, Name, and Score)
        private string GenerateETagForStudent(Student student)
        {
            // Simple version-based ETag, can be modified to include more properties or use a hash
            return $"W/\"{student.Id}-{student.Name}-{student.Score}\"";
        }

        public IQueryable<Student> GetAllStudents()
        {
            var students = _context.Students.AsQueryable();

            // Add ETag to each student (returning students with their ETags)
            foreach (var student in students)
            {
                student.ETag = GenerateETagForStudent(student);  // Ensure ETag is set for each student
            }

            return students;
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return null;

            // Generate ETag for the student
            student.ETag = GenerateETagForStudent(student);
            return student;
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            // Generate an ETag before saving the student
            student.ETag = GenerateETagForStudent(student);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student; // Return the student with its ETag
        }

        public async Task<Student?> UpdateStudentAsync(int id, Student student)
        {
            if (student == null || id != student.Id)
                return null;

            var existing = await _context.Students.FindAsync(id);
            if (existing == null)
                return null;

            // Update student properties
            _context.Entry(existing).CurrentValues.SetValues(student);

            // Regenerate the ETag for the updated student
            existing.ETag = GenerateETagForStudent(existing);

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool StudentExists(int id)
        {
            return _context.Students.Any(s => s.Id == id);
        }
    }
}

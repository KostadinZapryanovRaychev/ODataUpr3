using OdataSolution.Models;

namespace OdataSolution.Services
{
    public class StudentService : IStudentService
    {
        public IQueryable<Student> GetAllStudents()
        {
            var students = new List<Student>
            {
                new Student { Id = 1, Name = "Alice", Description = "Top student in math", Age = 20, Score = 95 },
                new Student { Id = 2, Name = "Bob", Description = "Enjoys science and coding", Age = 21, Score = 88 },
                new Student { Id = 3, Name = "Charlie", Description = "Athletic and hardworking", Age = 19, Score = 82 },
                new Student { Id = 4, Name = "Diana", Description = "Excellent in literature", Age = 22, Score = 91 },
                new Student { Id = 5, Name = "Ethan", Description = "Passionate about art and design", Age = 23, Score = 85 },
                new Student { Id = 6, Name = "Fiona", Description = "Leading debate club member", Age = 20, Score = 90 }
            };

            return students.AsQueryable();
        }
    }
}

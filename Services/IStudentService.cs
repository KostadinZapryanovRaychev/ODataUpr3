using OdataSolution.Models;

namespace OdataSolution.Services
{
    public interface IStudentService
    {
        IQueryable<Student> GetAllStudents();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student?> UpdateStudentAsync(int id, Student student);
        Task<bool> DeleteStudentAsync(int id);
    }
}

using OdataSolution.Models;

namespace OdataSolution.Services
{
    public interface IStudentService
    {
        IQueryable<Student> GetAllStudents();
    }
}

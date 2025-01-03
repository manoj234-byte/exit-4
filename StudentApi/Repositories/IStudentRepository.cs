using StudentApi.Models;
using System.Threading.Tasks;

public interface IStudentRepository
{
    Task<StudentResponse?> ConvertStudentTimeAsync(StudentRequest studentRequest);
}

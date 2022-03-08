using University.Models;

namespace University.Repositories
{
    public interface IStudentRepository
    {
        public void Add(Student student);
        public void Update(Student student);
        public void Delete(int id);
        public Student Get(int id);
        public IEnumerable<Student> GetAll();
    }
}

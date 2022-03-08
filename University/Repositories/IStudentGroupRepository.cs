using University.Models;

namespace University.Repositories
{
    public interface IStudentGroupRepository
    {
        public void Add(StudentGroup studentGroup);
        public void Update(StudentGroup studentGroup);
        public void Delete(int id);
        public StudentGroup Get(int id);
        public IEnumerable<StudentGroup> GetAll();
        public IEnumerable<StudentGroup> GetAllByGroup(int groupId);
    }
}

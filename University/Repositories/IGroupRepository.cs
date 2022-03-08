using University.Models;

namespace University.Repositories
{
    public interface IGroupRepository
    {
        public void Add(Group group);
        public void Update(Group group);
        public void Delete(int id);
        public Group Get(int id);
        public IEnumerable<Group> GetAll();
    }
}

using System.Data;
using System.Data.SqlClient;
using University.Models;

namespace University.Repositories
{
    public class StudentGroupRowSqlRepository : IStudentGroupRepository
    {
        private readonly string _connectionString;

        public StudentGroupRowSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(StudentGroup studentGroup)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [dbo].[StudentGroup]([GroupId], [StudentId]) VALUES (@groupId, @studentId)";
                    command.Parameters.Add("@groupId", SqlDbType.Int).Value = studentGroup.GroupId;
                    command.Parameters.Add("@studentId", SqlDbType.Int).Value = studentGroup.StudentId;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(StudentGroup studentGroup)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE [dbo].[StudentGroup] SET [GroupId] = @groupId, [StudentId] = @studentId WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = studentGroup.Id;
                    command.Parameters.Add("@groupId", SqlDbType.Int).Value = studentGroup.GroupId;
                    command.Parameters.Add("@studentId", SqlDbType.Int).Value = studentGroup.StudentId;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"DELETE FROM [dbo].[StudentGroup] WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public StudentGroup Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [GroupId], [StudentId] FROM [dbo].[StudentGroup] WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StudentGroup
                            {
                                Id = Convert.ToInt32(reader[nameof(StudentGroup.Id)]),
                                GroupId = Convert.ToInt32(reader[nameof(StudentGroup.GroupId)]),
                                StudentId = Convert.ToInt32(reader[nameof(StudentGroup.StudentId)])
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public IEnumerable<StudentGroup> GetAll()
        {
            List<StudentGroup> studentGroups = new List<StudentGroup>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [GroupId], [StudentId] FROM [dbo].[StudentGroup]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentGroups.Add(new StudentGroup
                            {
                                Id = Convert.ToInt32(reader[nameof(StudentGroup.Id)]),
                                GroupId = Convert.ToInt32(reader[nameof(StudentGroup.GroupId)]),
                                StudentId = Convert.ToInt32(reader[nameof(StudentGroup.StudentId)])
                            });
                        }
                    }
                }
            }

            return studentGroups;
        }

        public IEnumerable<StudentGroup> GetAllByGroup(int groupId)
        {
            List<StudentGroup> studentGroups = new List<StudentGroup>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [GroupId], [StudentId] FROM [dbo].[StudentGroup] WHERE [GroupId] = @groupId";
                    command.Parameters.Add("@groupId", SqlDbType.Int).Value = groupId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentGroups.Add(new StudentGroup
                            {
                                Id = Convert.ToInt32(reader[nameof(StudentGroup.Id)]),
                                GroupId = Convert.ToInt32(reader[nameof(StudentGroup.GroupId)]),
                                StudentId = Convert.ToInt32(reader[nameof(StudentGroup.StudentId)])
                            });
                        }
                    }
                }
            }

            return studentGroups;
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using University.Models;

namespace University.Repositories
{
    public class StudentRowSqlRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public StudentRowSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [dbo].[Student]([FirstName], [LastName]) VALUES (@firstName, @lastName)";
                    command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = student.FirstName;
                    command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = student.LastName;

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
                    command.CommandText = @"DELETE FROM [dbo].[Student] WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public Student Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [FirstName], [LastName] FROM [dbo].[Student] WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Student
                            {
                                Id = Convert.ToInt32(reader[nameof(Student.Id)]),
                                FirstName = Convert.ToString(reader[nameof(Student.FirstName)]),
                                LastName = Convert.ToString(reader[nameof(Student.LastName)])
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

        public IEnumerable<Student> GetAll()
        {
            List<Student> students = new List<Student>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [FirstName], [LastName] FROM [dbo].[Student]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            students.Add(new Student
                            {
                                Id = Convert.ToInt32(reader[nameof(Student.Id)]),
                                FirstName = Convert.ToString(reader[nameof(Student.FirstName)]),
                                LastName = Convert.ToString(reader[nameof(Student.LastName)])
                            });
                        }
                    }
                }
            }

            return students;
        }

        public void Update(Student student)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE [dbo].[Student] SET [FirstName] = @firstName, [LastName] = @lastName WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = student.Id;
                    command.Parameters.Add("@firstName", SqlDbType.NVarChar).Value = student.FirstName;
                    command.Parameters.Add("@lastName", SqlDbType.NVarChar).Value = student.LastName;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

using System.Data;
using System.Data.SqlClient;
using University.Models;

namespace University.Repositories
{
    public class GroupRowSqlRepository : IGroupRepository
    {
        private readonly string _connectionString;

        public GroupRowSqlRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Group group)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO [dbo].[Group]([Name]) VALUES (@name)";
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = group.Name;

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
                    command.CommandText = @"DELETE FROM [dbo].[Group] WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public Group Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [Name] FROM [dbo].[Group] WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Group
                            {
                                Id = Convert.ToInt32(reader[nameof(Group.Id)]),
                                Name = Convert.ToString(reader[nameof(Group.Name)])
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

        public IEnumerable<Group> GetAll()
        {
            List<Group> groups = new List<Group>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT [Id], [Name] FROM [dbo].[Group]";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            groups.Add(new Group
                            {
                                Id = Convert.ToInt32(reader[nameof(Group.Id)]),
                                Name = Convert.ToString(reader[nameof(Group.Name)])
                            });
                        }
                    }
                }
            }

            return groups;
        }

        public void Update(Group group)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"UPDATE [dbo].[Group] SET [Name] = @name WHERE [Id] = @id";
                    command.Parameters.Add("@id", SqlDbType.Int).Value = group.Id;
                    command.Parameters.Add("@name", SqlDbType.NVarChar).Value = group.Name;

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

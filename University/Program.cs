using University.Models;
using University.Repositories;

namespace University
{
    class Program
    {
        private static string _connectionString = @"Server=localhost\SQLEXPRESS;Database=University;Trusted_Connection=True;";

        private static IStudentRepository studentRepository = new StudentRowSqlRepository(_connectionString);
        private static IGroupRepository groupRepository = new GroupRowSqlRepository(_connectionString);
        private static IStudentGroupRepository studentGroupRepository = new StudentGroupRowSqlRepository(_connectionString);


        public static void Main (string[] args)
        {
            Console.WriteLine("Available commands:\n");
            Console.WriteLine("exit - close program");
            Console.WriteLine("add-student - add student");
            Console.WriteLine("add-group - add student group");
            Console.WriteLine("add-student-to-group - add student to student group");
            Console.WriteLine("show-students - show all students");
            Console.WriteLine("show-groups - show all group");
            Console.WriteLine("show-student-groups - show relationship group and stdent");
            Console.WriteLine("show-students-by-group-id - show all student in student group by student group id");
            Console.WriteLine("show-report - output a report with student amount in student groups\n");

            while (true)
            {
                Console.WriteLine();
                string command = Console.ReadLine();
                switch (command)
                {
                    case "exit":
                        return;
                    case "add-student":
                        AddStudent();
                        break;
                    case "add-group":
                        AddGroup();
                        break;
                    case "add-student-to-group":
                        AddStudentGroup();
                        break;
                    case "show-students":
                        ShowStudents();
                        break;
                    case "show-groups":
                        ShowGroups();
                        break;
                    case "show-student-groups":
                        ShowStudentGroup();
                        break;
                    case "show-students-by-group-id":
                        ShowStudentsByGroupId();
                        break;
                    case "show-report":
                        ShowReport();
                        break;

                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        private static void AddStudent()
        {
            Console.WriteLine("Enter first name:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Enter last name:");
            string lastName = Console.ReadLine();

            if ( String.IsNullOrWhiteSpace( firstName ) )
            {
                Console.WriteLine("First name can't be empty or consist only of white-space characters");
                return;
            }

            if ( String.IsNullOrWhiteSpace( lastName ) )
            {
                Console.WriteLine( "Last name can't be empty or consist only of white-space characters" );
                return;
            }

            studentRepository.Add(new Student
            {
                FirstName = firstName,
                LastName = lastName
            });

            Console.WriteLine("Success");
        }

        private static void AddGroup()
        {
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();

            if ( String.IsNullOrWhiteSpace( name ) )
            {
                Console.WriteLine( "Name can't be empty or consist only of white-space characters" );
                return;
            }

            groupRepository.Add(new Group
            {
                Name = name
            });

            Console.WriteLine("Success");
        }

        private static void AddStudentGroup()
        {
            int groupId;
            int studentId;

            try
            {
                Console.WriteLine("Enter group id:");
                groupId = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter student id:");
                studentId = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number");
                return;
            }


            if (groupRepository.Get(groupId) == null)
            {
                Console.WriteLine("Group doesn't exist");
                return;
            }

            if (studentRepository.Get(studentId) == null)
            {
                Console.WriteLine("Student doesn't exist");
                return;
            }


            IEnumerable<StudentGroup> studentGroups = studentGroupRepository.GetAllByGroup( groupId );

            if( studentGroups.Where(a => a.StudentId == studentId).Any() )
            {
                Console.WriteLine("Student already in this group.");
                return;
            }


            studentGroupRepository.Add(new StudentGroup
            {
                GroupId = groupId,
                StudentId = studentId
            });

            Console.WriteLine("Success");
        }

        private static void ShowStudents()
        {
            IEnumerable<Student> students = studentRepository.GetAll();

            if (!students.Any())
            {
                Console.WriteLine("No student's records!");
                return;
            }

            foreach (Student student in students )
            {
                Console.WriteLine($"Id: {student.Id}, First name: {student.FirstName}, Last name: {student.LastName}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowGroups()
        {
            IEnumerable<Group> groups = groupRepository.GetAll();

            if ( !groups.Any() )
            {
                Console.WriteLine("No group's records!");
                return;
            }

            foreach (Group group in groups )
            {
                Console.WriteLine($"Id: {group.Id}, Name: {group.Name}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowStudentGroup()
        {
            IEnumerable<StudentGroup> studentGroups = studentGroupRepository.GetAll();

            if ( !studentGroups.Any() )
            {
                Console.WriteLine("No student group's records!");
                return;
            }

            foreach ( StudentGroup studentGroup in studentGroups )
            {
                Console.WriteLine($"Id: {studentGroup.Id}, Group: {groupRepository.Get(studentGroup.GroupId).Name}, Student: {studentRepository.Get(studentGroup.StudentId).GetFullName()}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowStudentsByGroupId()
        {
            int groupId;

            try
            {
                Console.WriteLine("Enter group id:");
                groupId = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number");
                return;
            }

            Group group = groupRepository.Get(groupId);

            if (group == null)
            {
                Console.WriteLine("Group doesn't exist");
                return;
            }

            IEnumerable<StudentGroup> studentGroups = studentGroupRepository.GetAllByGroup( groupId );

            if ( !studentGroups.Any() )
            {
                Console.WriteLine("No student group's records!");
                return;
            }

            foreach (StudentGroup studentGroup in studentGroups )
            {
                Console.WriteLine($"Id: {studentGroup.Id}, Group: {group.Name}, Student: {studentRepository.Get(studentGroup.StudentId).GetFullName()}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowReport()
        {
            IEnumerable<Group> groups = groupRepository.GetAll();

            if ( !groups.Any() )
            {
                Console.WriteLine("No group's records!");
                return;
            }

            foreach ( Group group in groups )
            {
                Console.WriteLine($"Group: {group.Name}, Student amount: {studentGroupRepository.GetAllByGroup(group.Id).Count()}");
            }

            Console.WriteLine("Success");
        }
    }
}
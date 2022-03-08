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
            Console.WriteLine("show-student-groups-dev - show table dbo.StudentGroup");
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
                    case "show-student-groups-dev":
                        ShowStudentGroupsDev();
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


            studentGroupRepository.Add(new StudentGroup
            {
                GroupId = groupId,
                StudentId = studentId
            });

            Console.WriteLine("Success");
        }

        private static void ShowStudents()
        {
            foreach (Student student in studentRepository.GetAll())
            {
                Console.WriteLine($"Id: {student.Id}, First name: {student.FirstName}, Last name: {student.LastName}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowGroups()
        {
            foreach (Group group in groupRepository.GetAll())
            {
                Console.WriteLine($"Id: {group.Id}, Name: {group.Name}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowStudentGroupsDev()
        {
            foreach (StudentGroup studentGroup in studentGroupRepository.GetAll())
            {
                Console.WriteLine($"Id: {studentGroup.Id}, GroupId: {studentGroup.GroupId}, StudentId: {studentGroup.StudentId}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowStudentGroup()
        {
            foreach (StudentGroup studentGroup in studentGroupRepository.GetAll())
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

            foreach (StudentGroup studentGroup in studentGroupRepository.GetAllByGroup(groupId))
            {
                Console.WriteLine($"Id: {studentGroup.Id}, Group: {group.Name}, Student: {studentRepository.Get(studentGroup.StudentId).GetFullName()}");
            }

            Console.WriteLine("Success");
        }

        private static void ShowReport()
        {
            foreach (Group group in groupRepository.GetAll())
            {
                Console.WriteLine($"Group: {group.Name}, Student amount: {studentGroupRepository.GetAllByGroup(group.Id).Count()}");
            }

            Console.WriteLine("Success");
        }
    }
}
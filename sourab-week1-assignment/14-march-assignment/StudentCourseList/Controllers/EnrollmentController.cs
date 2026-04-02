using Microsoft.AspNetCore.Mvc;
using MVC_Demo_Project.Models;

namespace MVC_Demo_Project.Controllers
{
    public class EnrollmentController : Controller
    {
        private static readonly List<Student> Students =
        [
            new Student { StudentId = 1, Name = "Alice", Branch = "CSE" },
            new Student { StudentId = 2, Name = "Bob", Branch = "CSE" },
            new Student { StudentId = 3, Name = "Carol", Branch = "ECE" },
            new Student { StudentId = 4, Name = "David", Branch = "ME" },
            new Student { StudentId = 5, Name = "Eva", Branch = "IT" },
            new Student { StudentId = 6, Name = "Frank", Branch = "EEE" }
        ];

        private static readonly List<Course> Courses =
        [
            new Course { CourseId = 1, Title = "Data Structures", Credits = 4, Department = "CSE" },
            new Course { CourseId = 2, Title = "Algorithms", Credits = 4, Department = "CSE" },
            new Course { CourseId = 3, Title = "Databases", Credits = 3, Department = "CSE" },
            new Course { CourseId = 4, Title = "Web Dev", Credits = 3, Department = "CSE" },
            new Course { CourseId = 5, Title = "OS", Credits = 4, Department = "CSE" }
        ];

        private static readonly List<Enrollment> Enrollments =
        [
            new Enrollment { EnrollmentId = 1, StudentId = 1, CourseId = 1, Grade = "B", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 2, StudentId = 1, CourseId = 1, Grade = "A", AttemptNumber = 2 },
            new Enrollment { EnrollmentId = 3, StudentId = 1, CourseId = 2, Grade = "A-", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 4, StudentId = 1, CourseId = 3, Grade = "B+", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 5, StudentId = 2, CourseId = 1, Grade = "B", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 6, StudentId = 2, CourseId = 4, Grade = "A", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 7, StudentId = 2, CourseId = 5, Grade = "B+", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 8, StudentId = 3, CourseId = 2, Grade = "B+", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 9, StudentId = 3, CourseId = 4, Grade = "A-", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 10, StudentId = 4, CourseId = 3, Grade = "C", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 11, StudentId = 5, CourseId = 5, Grade = "A", AttemptNumber = 1 },
            new Enrollment { EnrollmentId = 12, StudentId = 6, CourseId = 1, Grade = "B-", AttemptNumber = 1 }
        ];

        static EnrollmentController()
        {
            foreach (var enrollment in Enrollments)
            {
                enrollment.Student = Students.First(s => s.StudentId == enrollment.StudentId);
                enrollment.Course = Courses.First(c => c.CourseId == enrollment.CourseId);
                enrollment.Student.Enrollments.Add(enrollment);
                enrollment.Course.Enrollments.Add(enrollment);
            }
        }

        public IActionResult Index()
        {
            var students = Students
                .Select(student => new StudentCoursesVM
                {
                    StudentId = student.StudentId,
                    Name = student.Name,
                    Branch = student.Branch,
                    CourseTitles = student.Enrollments
                        .Select(enrollment => enrollment.Course!.Title)
                        .Distinct()
                        .ToList()
                })
                .ToList();

            return View(students);
        }

        public IActionResult Details(int studentId)
        {
            var student = Students.FirstOrDefault(s => s.StudentId == studentId);
            if (student is null)
            {
                return NotFound();
            }

            var details = new StudentDetailVM
            {
                StudentId = student.StudentId,
                Name = student.Name,
                Branch = student.Branch,
                Courses = student.Enrollments
                    .GroupBy(enrollment => enrollment.CourseId)
                    .Select(group => group
                        .OrderByDescending(enrollment => enrollment.AttemptNumber)
                        .First())
                    .Select(enrollment => new CourseDetailVM
                    {
                        Title = enrollment.Course!.Title,
                        Credits = enrollment.Course.Credits,
                        Department = enrollment.Course.Department,
                        LatestGrade = enrollment.Grade
                    })
                    .ToList()
            };

            return View(details);
        }
    }
}

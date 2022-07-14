using System;
using System.Threading.Tasks;
using AuthDb;
using AuthServer.Extensions.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace AuthServer.Controllers
{
    [ApiController]
    public class FooController : ControllerBase
    {
        private AuthContext context;

        public FooController(AuthContext context)
        {
            this.context = context;
        }

        [HttpPost, Route("Auth/Foo")]
        //[Authorize(AuthenticationSchemes = SessionAuthenticationSchemeOptions.Name, Policy = "ServiceApi")]
        public async Task<ActionResult<string>> HandleAsync([FromBody] ArgumentData args)
        {
            return args.Command switch
            {
                "Create" => await CreateData(),
                "Select1" => await SelectData1(args.Id),
                "Select2" => await SelectData2(args.Id),
                "Select3" => await SelectData3(args.Id),
                "Select4" => await SelectData4(args.Id),
                _ => throw new InvalidOperationException(),
            };
        }

        private async Task<ActionResult<string>> SelectData1(int Id)
        {
            var sql = context.Enrollments
                .Include(model => model.Course)
                    .ThenInclude(model => model.CourseAssignments)
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToQueryString();
            
            var data = await context.Enrollments
                .Include(model => model.Course)
                    .ThenInclude(model => model.CourseAssignments)
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToListAsync();

            return data.ToString();
        }
        
        private async Task<ActionResult<string>> SelectData2(int Id)
        {
            var sql = context.Enrollments
                .Include(model => model.Course)
                    .ThenInclude(model => model.CourseAssignments)
                    .ThenInclude(model => model.Instructor)
                .Include(model => model.Course)
                    .ThenInclude(model => model.Department)
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToQueryString();
            
            var data = await context.Enrollments
                .Include(model => model.Course)
                    .ThenInclude(model => model.CourseAssignments)
                    .ThenInclude(model => model.Instructor)
                .Include(model => model.Course)
                    .ThenInclude(model => model.Department)
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToListAsync();

            return data.ToString();
        }
        
        private async Task<ActionResult<string>> SelectData3(int Id)
        {
            var sql = context.Enrollments
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToQueryString();
                
            var data = await context.Enrollments
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToListAsync();

            return data.ToString();
        }
        
        private async Task<ActionResult<string>> SelectData4(int Id)
        {
            var sql = context.Enrollments
                .Include(model => model.Course)
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToQueryString();
            
            var data = await context.Enrollments
                .Include(model => model.Course)
                .Include(model => model.Student)
                .Where(row => row.EnrollmentId == Id)
                .ToListAsync();

            return data.ToString();
        }

        private async Task<ActionResult<string>> CreateData()
        {
            if (context.Students.Any())
            {
                return "Already";
            }

            var students = await CreateStudents();
            
            var instructors = await CreateInstructors();
            
            var departments = await CreateDepartments(instructors);
            
            var courses = await CreateCourses(departments);
            
            await CreateOfficeAssignments(instructors);
            
            await CreateInstructors(courses, instructors);
            
            await CreateEnrollments(students, courses);

            return "Ok";
        }

        private async Task CreateEnrollments(Student[] students, Course[] courses)
        {
            var enrollments = new Enrollment[]
            {
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").CourseId,
                    Grade = Grade.A
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Microeconomics").CourseId,
                    Grade = Grade.C
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Alexander").Id,
                    CourseId = courses.Single(c => c.Title == "Macroeconomics").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Calculus").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Trigonometry").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Alonso").Id,
                    CourseId = courses.Single(c => c.Title == "Composition").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").CourseId
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Anand").Id,
                    CourseId = courses.Single(c => c.Title == "Microeconomics").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Barzdukas").Id,
                    CourseId = courses.Single(c => c.Title == "Chemistry").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Li").Id,
                    CourseId = courses.Single(c => c.Title == "Composition").CourseId,
                    Grade = Grade.B
                },
                new()
                {
                    StudentId = students.Single(s => s.LastName == "Justice").Id,
                    CourseId = courses.Single(c => c.Title == "Literature").CourseId,
                    Grade = Grade.B
                }
            };

            foreach (var e in enrollments)
            {
                var enrollmentInDatabase = await context.Enrollments
                    .Where(x => x.Student.Id == e.StudentId)
                    .Where(x => x.Course.CourseId == e.CourseId)
                    .SingleOrDefaultAsync();

                if (enrollmentInDatabase == null)
                {
                    await context.Enrollments.AddAsync(e);
                }
            }

            await context.SaveChangesAsync();
        }

        private async Task CreateInstructors(Course[] courses, Instructor[] instructors)
        {
            var courseInstructors = new CourseAssignment[]
            {
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Chemistry").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Chemistry").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Microeconomics").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Macroeconomics").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Zheng").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Calculus").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Trigonometry").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Composition").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new()
                {
                    CourseId = courses.Single(c => c.Title == "Literature").CourseId,
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
            };

            foreach (CourseAssignment ci in courseInstructors)
            {
                await context.CourseAssignments.AddAsync(ci);
            }

            await context.SaveChangesAsync();
        }

        private async Task CreateOfficeAssignments(Instructor[] instructors)
        {
            var officeAssignments = new OfficeAssignment[]
            {
                new() { InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id, Location = "Smith 17" },
                new() { InstructorId = instructors.Single(i => i.LastName == "Harui").Id, Location = "Gowan 27" },
                new() { InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id, Location = "Thompson 304" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                await context.OfficeAssignments.AddAsync(o);
            }

            await context.SaveChangesAsync();
        }

        private async Task<Course[]> CreateCourses(Department[] departments)
        {
            var courses = new Course[]
            {
                new()
                {
                    CourseId = 1050, Title = "Chemistry", Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "Engineering").DepartmentId
                },
                new()
                {
                    CourseId = 4022, Title = "Microeconomics", Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "Economics").DepartmentId
                },
                new()
                {
                    CourseId = 4041, Title = "Macroeconomics", Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "Economics").DepartmentId
                },
                new()
                {
                    CourseId = 1045, Title = "Calculus", Credits = 4,
                    DepartmentId = departments.Single(s => s.Name == "Mathematics").DepartmentId
                },
                new()
                {
                    CourseId = 3141, Title = "Trigonometry", Credits = 4,
                    DepartmentId = departments.Single(s => s.Name == "Mathematics").DepartmentId
                },
                new()
                {
                    CourseId = 2021, Title = "Composition", Credits = 3,
                    DepartmentId = departments.Single(s => s.Name == "English").DepartmentId
                },
                new()
                {
                    CourseId = 2042, Title = "Literature", Credits = 4,
                    DepartmentId = departments.Single(s => s.Name == "English").DepartmentId
                },
            };

            foreach (Course c in courses)
            {
                await context.Courses.AddAsync(c);
            }

            await context.SaveChangesAsync();

            return courses;
        }

        private async Task<Department[]> CreateDepartments(Instructor[] instructors)
        {
            var departments = new Department[]
            {
                new()
                {
                    Name = "English", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Abercrombie").Id
                },
                new()
                {
                    Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Fakhouri").Id
                },
                new()
                {
                    Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Harui").Id
                },
                new()
                {
                    Name = "Economics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"),
                    InstructorId = instructors.Single(i => i.LastName == "Kapoor").Id
                }
            };

            foreach (Department d in departments)
            {
                await context.Departments.AddAsync(d);
            }

            await context.SaveChangesAsync();

            return departments;
        }

        private async Task<Instructor[]> CreateInstructors()
        {
            var instructors = new Instructor[]
            {
                new() { FirstMidName = "Kim", LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
                new() { FirstMidName = "Fadi", LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06") },
                new() { FirstMidName = "Roger", LastName = "Harui", HireDate = DateTime.Parse("1998-07-01") },
                new() { FirstMidName = "Candace", LastName = "Kapoor", HireDate = DateTime.Parse("2001-01-15") },
                new() { FirstMidName = "Roger", LastName = "Zheng", HireDate = DateTime.Parse("2004-02-12") }
            };

            foreach (Instructor i in instructors)
            {
                await context.Instructors.AddAsync(i);
            }

            await context.SaveChangesAsync();

            return instructors;
        }

        private async Task<Student[]> CreateStudents()
        {
            var students = new Student[]
            {
                new() { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2010-09-01") },
                new() { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new() { FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2013-09-01") },
                new() { FirstMidName = "Gytis", LastName = "Barzdukas", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new() { FirstMidName = "Yan", LastName = "Li", EnrollmentDate = DateTime.Parse("2012-09-01") },
                new() { FirstMidName = "Peggy", LastName = "Justice", EnrollmentDate = DateTime.Parse("2011-09-01") },
                new() { FirstMidName = "Laura", LastName = "Norman", EnrollmentDate = DateTime.Parse("2013-09-01") },
                new() { FirstMidName = "Nino", LastName = "Olivetto", EnrollmentDate = DateTime.Parse("2005-09-01") }
            };

            foreach (Student s in students)
            {
                await context.Students.AddAsync(s);
            }
            
            await context.SaveChangesAsync();

            return students;
        }

        public sealed record ArgumentData(string Command, int Id);
    }
}
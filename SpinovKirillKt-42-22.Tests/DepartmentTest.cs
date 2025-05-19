using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Database;
using SpinovKirillKT_42_22.Interfaces;
using SpinovKirillKT_42_22.Models;
using SpinovKirillKT_42_22.Services.DepartmentServices;
using SpinovKirillKT_42_22.Services.TeacherServices;
using Xunit;

namespace SpinovKirillKt_42_22.Tests
{
    public class Test
    {
        private readonly DbContextOptions<TeacherLoadContext> _dbContextOptions;

        public Test()
        {
            var dbName = $"TestDatabase_Department_{Guid.NewGuid()}";
            _dbContextOptions = new DbContextOptionsBuilder<TeacherLoadContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            using (var ctx = new TeacherLoadContext(_dbContextOptions))
            {
                ctx.Database.EnsureDeleted();
                ctx.Database.EnsureCreated();
            }
        }


        [Fact]
        public async Task UnitTests()
        {
            using (var ctx = new TeacherLoadContext(_dbContextOptions))
            {
           
                var departmentService = new DepartmentService(ctx);
                var teacherService = new TeacherService(ctx);


                var departments = new List<Department>
        {
            new Department { Name = "Кафедра математики" },
            new Department { Name = "Кафедра физики" },
            //new Department { Name = "Кафедра информатики" }
        };
            var degrees = new List<AcademicDegree>
        {
            new AcademicDegree { Name = "Кандидат наук" },
            new AcademicDegree { Name = "Доктор наук" }
        };
            var posts = new List<Post>
        {
            new Post { Name = "Доцент" },
            new Post { Name = "Профессор" }
        };

                var teachers = new List<Teacher>
        {
            new Teacher
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Department = departments[0],
                Degree = degrees[0], 
                Post = posts[0]
            },
            new Teacher
            {
                FirstName = "Петр",
                LastName = "Петров",
                Department = departments[1],
                Degree = degrees[1], 
                Post = posts[1]   
            },
            new Teacher
            {
                FirstName = "Сидоров",
                LastName = "С",
                Department = departments[1],
                Degree = degrees[1],
                Post = posts[1]      
            }
        };
                var disciplines = new List<Discipline> {

                     new Discipline
                     {
                         Name = "Математический анализ"
                     },
                    new Discipline
                    {
                        Name = "Программирование"
                    }
                    };
        var loads = new List<Load>
        {
            new Load { Teacher = teachers[0], Discipline = disciplines[0], Hours = 10 },
            new Load { Teacher = teachers[1], Discipline = disciplines[0], Hours = 20 },
            new Load { Teacher = teachers[2], Discipline = disciplines[0], Hours = 30 },
            new Load { Teacher = teachers[1], Discipline = disciplines[1], Hours = 20 },
            new Load { Teacher = teachers[2], Discipline = disciplines[1], Hours = 30 }
        };

                await ctx.Departments.AddRangeAsync(departments);
                await ctx.Teachers.AddRangeAsync(teachers);
                await ctx.Disciplines.AddRangeAsync(disciplines);
                await ctx.Loads.AddRangeAsync(loads);
                await ctx.SaveChangesAsync();


                //var result = await departmentService.DepartmentByDiscipline("Программирование");
                var result = await teacherService.GetTeachersAsync("Кафедра математики");

                Assert.Equal(1, result.Count);
                //Assert.Contains("Кафедра информатики", result);
                //Assert.Contains("Кафедра физики", result);
                //Assert.DoesNotContain("Кафедра математики", result);
            }
        }

        [Theory]
        [InlineData("Математика", true)]
        [InlineData("Физика", true)]
        [InlineData("Высшая математика", true)]
        [InlineData("Иностранный язык (английский)", true)]
        [InlineData("123Математика", false)]
        [InlineData("Математика!", false)]
        [InlineData(" Математика", false)]
        [InlineData("Математика ", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        public async Task DepartmentByDiscipline_ValidatesDisciplineName(string disciplineName, bool isValid)
        {
            using (var ctx = new TeacherLoadContext(_dbContextOptions))
            {
                var departmentService = new DepartmentService(ctx);

                if (!isValid)
                {
                    if (string.IsNullOrEmpty(disciplineName))
                    {
                        await Assert.ThrowsAsync<ArgumentException>(() => departmentService.DepartmentByDiscipline(disciplineName));
                    }
                    else
                    {
                        var result = await departmentService.DepartmentByDiscipline(disciplineName);
                        Assert.Empty(result);
                    }
                }
                else
                {
                    var exception = await Record.ExceptionAsync(() => departmentService.DepartmentByDiscipline(disciplineName));
                    Assert.Null(exception);
                }
            }
        }

        [Theory]
        [InlineData("Математика", true)]
        [InlineData("Высшая математика", true)]
        [InlineData("Физика (теория поля)", true)]
        [InlineData("Физика-лаборатория", true)]
        [InlineData("История 20 века", true)]
        [InlineData("Математика  вторая часть", false)]
        [InlineData("123Математика", false)]
        [InlineData("Математика!", false)]
        [InlineData(" Математика", false)]
        [InlineData("Математика ", false)]
        [InlineData("МАТЕМАТИКА", false)]
        [InlineData("фИЗИКА", false)]
        [InlineData("", false)]
        public void DisciplineName_Validation_Test(string name, bool expected)
        {
            var regex = new Regex(@"^[А-ЯЁ](?:[а-яё0-9.,()\-]*(?: [а-яё0-9.,()\-]+)*)?[а-яё0-9.,()\-]$");
            Assert.Equal(expected, regex.IsMatch(name));
        }

        [Fact]
        public void IsValidDiscipline()
        {
            var testDiscipline = new Discipline
            {
                Name = "Математика"
            };

            var result = testDiscipline.IsValidDiscipline();

            Assert.True(result);
        }
    }
}

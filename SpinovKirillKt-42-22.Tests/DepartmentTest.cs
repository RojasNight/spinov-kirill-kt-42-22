using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Database;
using SpinovKirillKT_42_22.Models;
using SpinovKirillKT_42_22.Services.DepartmentServices;
using Xunit;

namespace SpinovKirillKt_42_22.Tests
{
    public class DepartmentTest
    {
        private readonly DbContextOptions<TeacherLoadContext> _dbContextOptions;

        public DepartmentTest()
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
        public async Task DepartmentByDiscipline_ReturnsCorrectDepartments()
        {
            using (var ctx = new TeacherLoadContext(_dbContextOptions))
            {
                // Инициализация сервиса
                var departmentService = new DepartmentService(ctx);

                // Создание тестовых данных
                var departments = new List<Department>
        {
            new Department { Name = "Кафедра математики" },
            new Department { Name = "Кафедра физики" },
            new Department { Name = "Кафедра информатики" }
        };

                var teachers = new List<Teacher>
        {
            new Teacher
            {
                FirstName = "Иван",
                LastName = "Иванов",
                Department = departments[2]  // Кафедра информатики
            },
            new Teacher
            {
                FirstName = "Петр",
                LastName = "Петров",
                Department = departments[1]  // Кафедра физики
            }
        };

                var discipline = new Discipline { Name = "Математический анализ" };

                var loads = new List<Load>
        {
            new Load { Teacher = teachers[0], Discipline = discipline, Hours = 10 },
            new Load { Teacher = teachers[1], Discipline = discipline, Hours = 10 }
        };

                // Групповое добавление данных
                await ctx.Departments.AddRangeAsync(departments);
                await ctx.Teachers.AddRangeAsync(teachers);
                await ctx.Disciplines.AddAsync(discipline);
                await ctx.Loads.AddRangeAsync(loads);
                await ctx.SaveChangesAsync();

                // Выполнение тестируемого метода
                var result = await departmentService.DepartmentByDiscipline("Математ");

                // Проверки
                Assert.Equal(2, result.Count);
                Assert.Contains("Кафедра информатики", result);
                Assert.Contains("Кафедра физики", result);
                Assert.DoesNotContain("Кафедра математики", result);
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
        [InlineData("Математика", true)]                      // Корректное
        [InlineData("Высшая математика", true)]               // Корректное (с пробелом внутри)
        [InlineData("Физика (теория поля)", true)]            // Корректное (со скобками)
        [InlineData("Физика-лаборатория", true)]              // Дефис разрешён, если после него нет цифр
        [InlineData("История 20 века", true)]                 // Цифры разрешены, если не после дефиса
        [InlineData("123Математика", false)]                  // Цифры в начале — запрещено
        [InlineData("Математика!", false)]                    // Спецсимволы — запрещено
        [InlineData(" Математика", false)]                    // Пробел в начале — запрещено
        [InlineData("Математика ", false)]                    // Пробел в конце — запрещено
        [InlineData("МАТЕМАТИКА", false)]                     // Верхний регистр — запрещено
        [InlineData("фИЗИКА", false)]                         // Смешанный регистр (кроме первой буквы) — запрещено
        [InlineData("", false)]                               // Пустая строка — запрещено
        public void DisciplineName_Validation_Test(string name, bool expected)
        {
            var regex = new Regex(@"^[А-ЯЁ][а-яё0-9.,()\-]*(?<!-\d+)(?: [а-яё0-9.,()\-]+)*[а-яё0-9.,()\-]$");
            Assert.Equal(expected, regex.IsMatch(name));
        }
    }
}

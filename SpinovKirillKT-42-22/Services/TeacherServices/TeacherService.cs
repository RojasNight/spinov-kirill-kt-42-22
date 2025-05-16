using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Database;
using SpinovKirillKT_42_22.Filters.TeacherFilter;
using SpinovKirillKT_42_22.Interfaces;
using SpinovKirillKT_42_22.Models;
using SpinovKirillKT_42_22.Models.DTO;

namespace SpinovKirillKT_42_22.Services.TeacherServices
{
    public class TeacherService : ITeacherService
    {
        private readonly TeacherLoadContext _context;

        public TeacherService(TeacherLoadContext context)
        {
            _context = context;
        }

        public async Task<List<TeacherFilter>> GetTeachersAsync(string departmentName = null, string degreeName = null, string PostName = null)
        {
            var query = _context.Teachers
                .Include(t => t.Degree)
                .Include(t => t.Post)
                .Include(t => t.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(departmentName))
            {
                query = query.Where(t => t.Department.Name == departmentName);
            }

            if (!string.IsNullOrEmpty(degreeName))
            {
                query = query.Where(t => t.Degree.Name == degreeName);
            }

            if (!string.IsNullOrEmpty(PostName))
            {
                query = query.Where(t => t.Post.Name == PostName);
            }

            var teachers = await query.Select(t => new TeacherFilter
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Degree = t.Degree.Name,
                Post = t.Post.Name,
                Department = t.Department.Name
            }).ToListAsync();

            return teachers;
        }

        public async Task<TeacherDto> GetTeacherByIdAsync(int id)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Degree)
                .Include(t => t.Post)
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
            {
                return null;
            }


            return new TeacherDto
            {
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                PostId = teacher.PostId,
                DegreeId = teacher.DegreeId,
                DepartmentId = teacher.DepartmentId
            };
        }

        public async Task<TeacherResponseDto> AddTeacherAsync(string firstName, string lastName, int PostId, int degreeId, int? departmentId)
        {



            var PostExists = await _context.Posts.AnyAsync(p => p.Id == PostId);
            if (!PostExists)
            {
                throw new ArgumentException($"Должности с id {PostId} не существует.");
            }

            var degreeExists = await _context.AcademicDegrees.AnyAsync(d => d.Id == degreeId);
            if (!degreeExists)
            {
                throw new ArgumentException($"Степени с id {degreeId} не существует.");
            }

            if (departmentId.HasValue)
            {
                var departmentExists = await _context.Departments.AnyAsync(d => d.Id == departmentId.Value);
                if (!departmentExists)
                {
                    throw new ArgumentException($"Кафедры с id {departmentId} не существует.");
                }
            }


            var teacher = new Teacher
            {
                FirstName = firstName,
                LastName = lastName,
                PostId = PostId,
                DegreeId = degreeId,
                DepartmentId = departmentId
            };

            if (!teacher.IsValid())
            {
                throw new ArgumentException("Имя и фамилия преподавателя должны начинаться с заглавной буквы.");
            }

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();


            var degree = await _context.AcademicDegrees.FindAsync(degreeId);
            var Post = await _context.Posts.FindAsync(PostId);
            var department = departmentId.HasValue ? await _context.Departments.FindAsync(departmentId.Value) : null;


            var responseDto = new TeacherResponseDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                DegreeId = teacher.DegreeId,
                Degree = degree.Name,
                PostId = teacher.PostId,
                Post = Post.Name,
                DepartmentId = teacher.DepartmentId,
                Department = department.Name,
                Loads = new List<Load>()
            };

            return responseDto;
        }

        public async Task<TeacherResponseDto> UpdateTeacherAsync(int id, string firstName, string lastName, int PostId, int degreeId, int? departmentId)
        {


            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null)
            {
                return null;
            }


            var PostExists = await _context.Posts.AnyAsync(p => p.Id == PostId);
            if (!PostExists)
            {
                throw new ArgumentException($"Должности с id {PostId} не существует.");
            }


            var degreeExists = await _context.AcademicDegrees.AnyAsync(d => d.Id == degreeId);
            if (!degreeExists)
            {
                throw new ArgumentException($"Степени с id {degreeId} не существует.");
            }


            if (departmentId.HasValue)
            {
                var departmentExists = await _context.Departments.AnyAsync(d => d.Id == departmentId.Value);
                if (!departmentExists)
                {
                    throw new ArgumentException($"Кафедры с id {departmentId} не существует.");
                }
            }


            teacher.FirstName = firstName;
            teacher.LastName = lastName;
            teacher.PostId = PostId;
            teacher.DegreeId = degreeId;
            teacher.DepartmentId = departmentId;

            if (!teacher.IsValid())
            {
                throw new ArgumentException("Имя и фамилия преподавателя должны начинаться с заглавной буквы.");
            }

            await _context.SaveChangesAsync();


            var degree = await _context.AcademicDegrees.FindAsync(degreeId);
            var Post = await _context.Posts.FindAsync(PostId);
            var department = departmentId.HasValue ? await _context.Departments.FindAsync(departmentId.Value) : null;


            var responseDto = new TeacherResponseDto
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                DegreeId = teacher.DegreeId,
                Degree = degree.Name,
                PostId = teacher.PostId,
                Post = Post.Name,
                DepartmentId = teacher.DepartmentId,
                Department = department.Name,
                Loads = new List<Load>()
            };

            return responseDto;
        }


        public async Task<bool> DeleteTeacherAsync(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher != null)
            {
                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}

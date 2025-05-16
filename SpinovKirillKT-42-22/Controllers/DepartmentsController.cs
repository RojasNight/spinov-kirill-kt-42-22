using Microsoft.AspNetCore.Mvc;
using SpinovKirillKT_42_22.Models;
using SpinovKirillKT_42_22.Models.DTO;
using SpinovKirillKT_42_22.Services.DepartmentServices;
using System;
using System.Threading.Tasks;

namespace SpinovKirillKT_42_22.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly DepartmentService _departmentService;

        public DepartmentsController(DepartmentService departmentService)
        {
            _departmentService = departmentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetDepartments([FromQuery] DateTime? foundedAfter = null, [FromQuery] int? minTeacherCount = null)
        {
            var departments = await _departmentService.GetDepartmentsAsync(foundedAfter, minTeacherCount);
            return Ok(departments);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
        {
            if (createDepartmentDto == null)
            {
                return BadRequest();
            }

            var department = new Department
            {
                Name = createDepartmentDto.Name,
                FoundedDate = createDepartmentDto.FoundedDate
            };

            await _departmentService.AddDepartmentAsync(department);
            return CreatedAtAction(nameof(GetDepartments), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] UpdateDepartmentDto updateDepartmentDto)
        {
            if (updateDepartmentDto == null)
            {
                return BadRequest(new { message = "Данные для кафедры обновления не могут быть пустыми" });
            }

            var department = new Department
            {
                Id = id,
                Name = updateDepartmentDto.Name,
                FoundedDate = updateDepartmentDto.FoundedDate,
                HeadId = updateDepartmentDto.HeadId
            };

            var updatedDepartment = await _departmentService.UpdateDepartmentAsync(department);
            if (updatedDepartment == null)
            {
                return NotFound(new { message = "Кафедра не найдена" });
            }


            var updatedDepartmentDto = new CreateDepartmentDto
            {
                Name = updatedDepartment.Name,
                FoundedDate = updatedDepartment.FoundedDate,
                HeadId = updatedDepartment.HeadId
            };

            return Ok(updatedDepartmentDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Кафедра не найдена" });
            }

            return NoContent();
        }
    }
}

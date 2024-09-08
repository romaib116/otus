using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Контроллер сотрудников
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeesController(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Сохранить нового сотрудника
        /// </summary>
        /// <param name="employeeRequest">Запрос</param>
        /// <returns>ГУИД сотрудника</returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> SaveEmployee([FromBody]EmployeeRequest employeeRequest)
        {
            var employee = new Employee()
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email
            };

            return Ok(await _employeeRepository.SaveAsync(employee));
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <param name="id">Id</param>
        [HttpDelete("{id:guid}")]
        public async Task DeleteEmployee(Guid id)
        {
            await _employeeRepository.RemoveAsync(id);
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="employee">Сотрудник</param>
        [HttpPut]
        public async Task UpdateEmployee([FromHeader]Guid id, [FromBody]Employee employee)
        {
            employee.Id = id;
            await _employeeRepository.UpdateAsync(employee);
        }
    }
}
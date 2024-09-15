using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Mappers;
using PromoCodeFactory.WebHost.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public async Task<ActionResult<IList<EmployeeShortResponse>>> GetEmployeesAsync(CancellationToken cancellationToken)
        {
            var employees = await _employeeRepository.GetAllAsync(cancellationToken);

            var employeesModelList = employees.Select(x => x.MapToSlimDto());

            return Ok(employeesModelList);
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var employee = await _employeeRepository.GetByIdAsync(id, cancellationToken);

            if (employee == null)
                return NotFound();

            var employeeModel = employee.MapToDto();

            return Ok(employeeModel);
        }

        /// <summary>
        /// Сохранить нового сотрудника
        /// </summary>
        /// <param name="employeeRequest">Запрос</param>
        /// <returns>ГУИД сотрудника</returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> SaveEmployee(EmployeeRequest employeeRequest, CancellationToken cancellationToken)
        {
            var employee = employeeRequest.MapFromDto();
            return Ok(await _employeeRepository.SaveAsync(employee, cancellationToken));
        }

        /// <summary>
        /// Удалить сотрудника
        /// </summary>
        /// <param name="id">Id</param>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteEmployee(Guid id, CancellationToken cancellationToken)
        {
            await _employeeRepository.RemoveAsync(id, cancellationToken);
            return Ok(null);
        }

        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="employee">Сотрудник</param>
        [HttpPut]
        public async Task<ActionResult> UpdateEmployee(Guid id, Employee employee, CancellationToken cancellationToken)
        {
            employee.Id = id;
            await _employeeRepository.UpdateAsync(employee, cancellationToken);
            return Ok(null);
        }
    }
}
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using System.Linq;

namespace PromoCodeFactory.WebHost.Mappers
{
    public static class EmployeeMapper
    {
        public static Employee MapFromDto(this EmployeeRequest request)
        {
            var employee = new Employee();
            employee.FirstName = request.FirstName;
            employee.LastName = request.LastName;
            employee.Email = request.Email;

            return employee;
        }

        public static EmployeeResponse MapToDto(this Employee response)
        {
            var slim = response.MapToSlimDto();

            var employeeModel = (EmployeeResponse)slim;
            employeeModel.Roles = response.Roles.Select(x => new RoleItemResponse()
            {
                Name = x.Name,
                Description = x.Description
            }).ToList();
            employeeModel.AppliedPromocodesCount = response.AppliedPromocodesCount;

            return employeeModel;
        }
        public static EmployeeShortResponse MapToSlimDto(this Employee response)
        {
            var employeeModel = new EmployeeShortResponse()
            {
                Id = response.Id,
                Email = response.Email,
                FullName = response.FullName,
            };

            return employeeModel;
        }
    }
}

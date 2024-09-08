using System.Collections.Generic;

namespace PromoCodeFactory.WebHost.Models
{
    public class EmployeeResponse : EmployeeShortResponse
    {
        public List<RoleItemResponse> Roles { get; set; }

        public int AppliedPromocodesCount { get; set; }
    }
}
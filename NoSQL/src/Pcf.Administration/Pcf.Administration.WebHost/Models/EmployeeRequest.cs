using System;
using System.Collections.Generic;
using Pcf.Administration.Core.Domain.Administration;

namespace Pcf.Administration.WebHost.Models
{
    public class EmployeeRequest
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
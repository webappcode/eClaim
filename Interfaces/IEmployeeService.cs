using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace eClaim.Interfaces
{
    public interface IEmployeeService
    {
        IEnumerable<SelectListItem> GetEmployees();
    }
}

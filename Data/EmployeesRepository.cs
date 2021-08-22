using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using eClaim.Interfaces;

namespace eClaim.Data
{
    public class EmployeesRepository : IEmployeeService
    {
        private ApplicationDbContext db;

        public EmployeesRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public IEnumerable<SelectListItem> GetEmployees()
        {
            List<SelectListItem> employees = db.Employees.AsNoTracking()
                .OrderBy(n => n.FirstName).ThenBy(n => n.LastName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.EmployeeID.ToString(),
                        Text = n.FirstName + " " + n.LastName
                    }).ToList();
            var employeetip = new SelectListItem()
            {
                Value = null,
                Text = "--- select employee ---"
            };
            employees.Insert(0, employeetip);
            return new SelectList(employees, "Value", "Text");
        }
    }
}
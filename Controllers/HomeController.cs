using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using eClaim.Models;
using eClaim.Interfaces;

namespace eClaim.Controllers
{
    public class HomeController : Controller
    {
        private readonly IExpenseService expenseService;
        private readonly IEmployeeService employeeService;
        private readonly IExchRateService exchRateService;

        public HomeController(IExpenseService _expenseService, IEmployeeService _employeeService, IExchRateService _exchRateService)
        {
            expenseService = _expenseService;
            employeeService = _employeeService;
            exchRateService = _exchRateService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getExpenses()
        {
            string search = Request.Form.GetValues("search[value]").FirstOrDefault();
            string draw = Request.Form.GetValues("draw").FirstOrDefault();
            string order = Request.Form.GetValues("order[0][column]").FirstOrDefault();
            string orderDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            int startRec = Convert.ToInt32(Request.Form.GetValues("start").FirstOrDefault());
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length").FirstOrDefault());

            List<ExpViewModel> data = new List<ExpViewModel>();
            data = expenseService.GetExpenses();

            // Total record count.    
            int totalRecords = data.Count;
            // Verification.    
            if (!string.IsNullOrEmpty(search) && !string.IsNullOrWhiteSpace(search))
            {
                // Apply search    
                data = data.Where(p => (p.Reference ?? "").ToLower().Contains(search.ToLower())
                        || (p.RequesterName ?? "").ToLower().Contains(search.ToLower())
                        || p.Date.Contains(search.ToLower())
                        || p.Amount.Contains(search.ToLower())
                        || (p.ApproverName ?? "").ToLower().Contains(search.ToLower())
                        || (p.ApprovedDate ?? "").ToLower().Contains(search.ToLower())
                        || (p.Status ?? "").ToLower().Contains(search.ToLower())).ToList();
            }

            // Sorting.    
            int col_order = Convert.ToInt32(order);
            switch (col_order)
            {
                case 1:
                    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.RequesterName).ToList() : data.OrderBy(p => p.RequesterName).ToList();
                    break;
                case 2:
                    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Date).ToList() : data.OrderBy(p => p.Date).ToList();
                    break;
                case 3:
                    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Amount).ToList() : data.OrderBy(p => p.Amount).ToList();
                    break;
                case 4:
                    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ApproverName).ToList() : data.OrderBy(p => p.ApproverName).ToList();
                    break;
                case 5:
                    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ApprovedDate).ToList() : data.OrderBy(p => p.ApprovedDate).ToList();
                    break;
                default:
                    data = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Reference).ToList() : data.OrderBy(p => p.Reference).ToList();
                    break;
            }

            // Filter record count.    
            int recFilter = data.Count;
            // Apply pagination.    
            data = data.Skip(startRec).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSingleExpense(int expenseId)
        {
            var detail = expenseService.GetSingleExpense(expenseId);

            return Json(new { result = detail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getSingleExpenseDetail(int detailId)
        {
            var detail = expenseService.GetSingleExpenseDetail(detailId);

            return Json(new { result = detail }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult saveExpense(ExpViewModel model)
        {
            try
            {
                bool saved = expenseService.SaveExpense(model);
                if (saved)
                {
                    return Json(new { error = false, message = "Record saved successfully" });
                }
            }
            catch (ApplicationException ex)
            {
                return Json(new { error = true, message = ex.Message });
            }
            return Json(new { error = true, message = "An unknown error has occured" });
        }

        public ActionResult saveDetail(ExpDetailsViewModel model)
        {
            try
            {
                bool saved = expenseService.SaveDetail(model);
                if (saved)
                {
                    return Json(new { error = false, message = "Record saved successfully" });
                }
            }
            catch (ApplicationException ex)
            {
                return Json(new { error = true, message = ex.Message });
            }
            return Json(new { error = true, message = "An unknown error has occured" });
        }

        public ActionResult deleteExpenseItem(int detailId)
        {
            try
            {
                bool deleted = expenseService.DeleteExpenseItem(detailId);
                if (deleted)
                {
                    return Json(new { error = false, message = "Record deleted successfully" });
                }
            }
            catch (ApplicationException ex)
            {
                return Json(new { error = true, message = ex.Message });
            }
            return Json(new { error = true, message = "An unknown error has occured" });
        }

        [HttpGet]
        public ActionResult getEmployees()
        {
            IEnumerable<SelectListItem> employees = employeeService.GetEmployees();
            return Json(employees, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getRateDetails(string baseCurrenry)
        {
            var objRate = exchRateService.GetExchangeRates(baseCurrenry);
            return Json(objRate, JsonRequestBehavior.AllowGet);
        }
    }
}
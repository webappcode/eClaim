using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eClaim.Models;
using eClaim.Interfaces;

namespace eClaim.Data
{
    public class ExpensesRepository : IExpenseService
    {
        private ApplicationDbContext db;

        public ExpensesRepository(ApplicationDbContext _db)
        {
            db = _db;
        }

        public List<ExpViewModel> GetExpenses()
        {
            try
            {
                List<ExpViewModel> expenseMasters = new List<ExpViewModel>();
                expenseMasters = db.uvwExpenseMasters.AsNoTracking().ToList()
                                .Select(x =>
                                    new ExpViewModel
                                    {
                                        Reference = x.Reference,
                                        ExpenseID = x.ExpenseID,
                                        RequesterName = x.RequesterName,
                                        Status = x.StatusDesc,
                                        Date = x.Date,
                                        Amount = x.Amount.ToString(),
                                        ApproverName = x.ApproverName,
                                        ApprovedDate = x.ApprovedDate
                                    }).ToList();

                return expenseMasters;
            }
            catch 
            {
                throw;
            }
        }

        public ExpDetailsViewModel GetSingleExpenseDetail(int detailId)
        {
            ExpDetailsViewModel result = new ExpDetailsViewModel();
            result = db.uvwExpenseDetails.AsNoTracking()
                .Where(d => d.DetailID == detailId)
                .Select(d =>
                    new ExpDetailsViewModel
                    {
                        DetailID = d.DetailID,
                        Date = d.Date,
                        CostCenter = d.CostCenter.ToString(),
                        GLCode = d.GLCode.ToString(),
                        Description = d.Description,
                        Currency = d.Currency,
                        Gst = d.Gst.ToString(),
                        ClaimAmount = d.ClaimAmount.ToString(),
                        ClaimCurrAmount = d.ClaimCurrAmount.ToString(),
                        ExhangeRate = d.ExhangeRate.ToString(),
                        Status = d.StatusDesc
                    }).SingleOrDefault();

            return result;
        }

        public ExpViewModel GetSingleExpense(int expenseId)
        {
            var model = db.uvwExpenseMasters.AsNoTracking()
                            .Where(a => a.ExpenseID == expenseId)
                        .SingleOrDefault();

            if (model != null)
            {
                ExpViewModel expDisplay = new ExpViewModel();
                expDisplay.Reference = model.Reference;
                expDisplay.ExpenseID = model.ExpenseID;
                expDisplay.RequesterID = model.RequesterID;
                expDisplay.RequesterName = model.RequesterName;
                expDisplay.BranchCode = model.BranchCode;
                expDisplay.BankAccName = model.BankAccName;
                expDisplay.BankBranch = model.BankBranch;
                expDisplay.BankAccount = model.BankAccount;
                expDisplay.Date = model.Date;
                expDisplay.Amount = model.Amount.ToString();
                expDisplay.ApproverID = model.ApproverID.ToString();
                expDisplay.ApprovedDate = model.ApprovedDate;
                expDisplay.Status = model.StatusDesc;
                expDisplay.ExpDetails = db.uvwExpenseDetails.AsNoTracking()
                                    .Where(d => d.ExpenseID == expenseId)
                                    .Select(d =>
                                        new ExpDetailsViewModel
                                        {
                                            DetailID = d.DetailID,
                                            Date = d.Date,
                                            CostCenter = d.CostCenter.ToString(),
                                            CostCenterDesc = d.CostCenterDesc,
                                            GLCode = d.GLCode.ToString(),
                                            GLCodeDesc = d.GLCodeDesc,
                                            Description = d.Description ?? "",
                                            Currency = d.Currency,
                                            Gst = d.Gst.ToString(),
                                            ClaimAmount = d.ClaimAmount.ToString(),
                                            ClaimCurrAmount = d.ClaimCurrAmount.ToString(),
                                            ExhangeRate = d.ExhangeRate.ToString(),
                                            Status = d.StatusDesc
                                        }).ToList();
                return expDisplay;
            }
            return null;
        }

        public bool SaveExpense(ExpViewModel exp)
        {
            if (exp != null)
            {
                try
                {
                    ExpenseMaster expenseMaster;
                    ExpenseDetail expenseDetail;

                    if (exp.ExpenseID == 0)
                        expenseMaster = new ExpenseMaster();
                    else
                        expenseMaster = db.ExpenseMasters.SingleOrDefault(e => e.ExpenseID == exp.ExpenseID);

                    if (expenseMaster != null)
                    {
                        expenseMaster.Reference = exp.Reference;
                        expenseMaster.RequesterID = exp.RequesterID;
                        expenseMaster.ApproverID = !string.IsNullOrEmpty(exp.ApproverID) ? Convert.ToInt32(exp.ApproverID) : (int?)null;
                        expenseMaster.Date = DateTime.Now;
                        expenseMaster.BranchCode = exp.BranchCode;
                        expenseMaster.BankAccName = exp.BankAccName;
                        expenseMaster.BankAccount = exp.BankAccount;
                        expenseMaster.BankBranch = exp.BankBranch;
                        expenseMaster.Amount = !string.IsNullOrEmpty(exp.Amount) ? decimal.Parse(exp.Amount) : 0;
                        expenseMaster.Status = 1;

                        if (exp.ExpenseID == 0)
                            db.ExpenseMasters.Add(expenseMaster);
                        else
                            db.Entry(expenseMaster).State = System.Data.Entity.EntityState.Modified;
                    }

                    //Process Expense details
                    if (exp.ExpDetails.Any())
                    {
                        foreach (var item in exp.ExpDetails)
                        {
                            if (item.DetailID == 0)
                                expenseDetail = new ExpenseDetail();
                            else
                                expenseDetail = db.ExpenseDetails.SingleOrDefault(e => e.ExpenseID == exp.ExpenseID && e.DetailID == item.DetailID);
                            expenseDetail.ExpenseID = exp.ExpenseID;
                            expenseDetail.Date = DateTime.Parse(item.Date);
                            expenseDetail.CostCenter = int.Parse(item.CostCenter);
                            expenseDetail.GLCode = int.Parse(item.GLCode);
                            expenseDetail.Description = item.Description;
                            expenseDetail.Currency = item.Currency;
                            expenseDetail.Gst = decimal.Parse(item.Gst);
                            expenseDetail.ExhangeRate = decimal.Parse(item.ExhangeRate);
                            expenseDetail.ClaimCurrAmount = decimal.Parse(item.ClaimCurrAmount);
                            expenseDetail.ClaimAmount = decimal.Parse(item.ClaimAmount);
                            expenseDetail.Status = 1;

                            if (item.DetailID == 0)
                                db.ExpenseDetails.Add(expenseDetail);
                            else
                                db.Entry(expenseDetail).State = System.Data.Entity.EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    // Log exception
                    throw;
                }
            }
            return false;
        }

        public bool SaveDetail(ExpDetailsViewModel exp)
        {
            if (exp != null)
            {
                try
                {
                    ExpenseDetail expenseDetail;

                    if (exp.DetailID == 0)
                        expenseDetail = new ExpenseDetail();
                    else
                        expenseDetail = db.ExpenseDetails.SingleOrDefault(e => e.DetailID == exp.DetailID);
                    expenseDetail.Date = DateTime.Parse(exp.Date);
                    expenseDetail.CostCenter = int.Parse(exp.CostCenter);
                    expenseDetail.GLCode = int.Parse(exp.GLCode);
                    expenseDetail.Description = exp.Description;
                    expenseDetail.Currency = exp.Currency;
                    expenseDetail.Gst = decimal.Parse(exp.Gst);
                    expenseDetail.ExhangeRate = decimal.Parse(exp.ExhangeRate);
                    expenseDetail.ClaimCurrAmount = decimal.Parse(exp.ClaimCurrAmount);
                    expenseDetail.ClaimAmount = decimal.Parse(exp.ClaimAmount);
                    expenseDetail.Status = 1;

                    if (expenseDetail.DetailID == 0)
                        db.ExpenseDetails.Add(expenseDetail);
                    else
                        db.Entry(expenseDetail).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    // Log exception
                    throw;
                }
            }
            return false;
        }

        public bool DeleteExpenseItem(int DetailId)
        {
            var detail = db.ExpenseDetails.Find(DetailId);
            if (null != detail)
            {
                db.ExpenseDetails.Remove(detail);
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
using System;
using System.Collections.Generic;

namespace eClaim.Models
{
    public class ExpViewModel
    {
        public ExpViewModel()
        {
            this.ExpDetails = new List<ExpDetailsViewModel>();
        }
        public int ExpenseID { get; set; }
        public int RequesterID { get; set; }
        public string ApproverID { get; set; }
        public string Reference { get; set; }
        public string Date { get; set; }
        public string BranchCode { get; set; }
        public string Amount { get; set; }
        public string Status { get; set; }
        public string BankAccount { get; set; }
        public string BankBranch { get; set; }
        public string BankAccName { get; set; }
        public string RequesterName { get; set; }
        public string ApproverName { get; set; }
        public string ApprovedDate { get; set; }

        public virtual ICollection<ExpDetailsViewModel> ExpDetails { get; set; }
    }

    public class ExpDetailsViewModel
    {
        public int DetailID { get; set; }
        public int ExpenseID { get; set; }
        public string Date { get; set; }
        public string CostCenter { get; set; }
        public string CostCenterDesc { get; set; }
        public string GLCode { get; set; }
        public string GLCodeDesc { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string Gst { get; set; }
        public string ExhangeRate { get; set; }
        public string ClaimCurrAmount { get; set; }
        public string ClaimAmount { get; set; }
        public string Status { get; set; }
    }
}
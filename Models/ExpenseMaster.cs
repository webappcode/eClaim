//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace eClaim.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ExpenseMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ExpenseMaster()
        {
            this.ExpenseDetails = new HashSet<ExpenseDetail>();
        }
    
        public int ExpenseID { get; set; }
        public int RequesterID { get; set; }
        public Nullable<int> ApproverID { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string Reference { get; set; }
        public System.DateTime Date { get; set; }
        public string BranchCode { get; set; }
        public decimal Amount { get; set; }
        public byte Status { get; set; }
        public string BankAccount { get; set; }
        public string BankBranch { get; set; }
        public string BankAccName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExpenseDetail> ExpenseDetails { get; set; }
    }
}
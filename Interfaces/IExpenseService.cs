using System;
using System.Collections.Generic;
using eClaim.Models;

namespace eClaim.Interfaces
{
    public interface IExpenseService
    {
        List<ExpViewModel> GetExpenses();
        ExpDetailsViewModel GetSingleExpenseDetail(int detailId);
        ExpViewModel GetSingleExpense(int expenseId);
        bool SaveExpense(ExpViewModel exp);
        bool SaveDetail(ExpDetailsViewModel exp);
        bool DeleteExpenseItem(int DetailId);
    }
}

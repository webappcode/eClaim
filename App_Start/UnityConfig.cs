using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using eClaim.Interfaces;
using eClaim.Data;

namespace eClaim
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IExpenseService, ExpensesRepository>();
            container.RegisterType<IEmployeeService, EmployeesRepository>();
            container.RegisterType<IExchRateService, ExchRateRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
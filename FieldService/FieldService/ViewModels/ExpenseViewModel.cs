using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// ViewModel for expense
    /// </summary>
    public class ExpenseViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<Expense> expenses;

        public ExpenseViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of expenses
        /// </summary>
        public List<Expense> Expenses
        {
            get { return expenses; }
            set { expenses = value; OnPropertyChanged ("Expenses"); }
        }

        /// <summary>
        /// Loads the list of expenses
        /// </summary>
        public Task LoadExpensesAsync (Assignment assignment)
        {
            return service
                .GetExpensesForAssignmentAsync (assignment)
                .ContinueOnUIThread (t => Expenses = t.Result);
        }

        /// <summary>
        /// Saves an expense report
        /// </summary>
        public Task SaveExpenseAsync (Assignment assignment, Expense expense)
        {
            bool newItem = expense.Id == 0;

            return service.SaveExpense (expense)
                .ContinueWith (t => {
                    if (newItem)
                        expenses.Add (expense);
                    CalculateExpenses (assignment);
                });
        }

        /// <summary>
        /// Deletes an expense report
        /// </summary>
        public Task DeleteExpenseAsync (Assignment assignment, Expense expense)
        {
            return service.DeleteExpense (expense)
                .ContinueWith (t => {
                    expenses.Remove (expense);
                    CalculateExpenses (assignment);
                });
        }

        /// <summary>
        /// Just a quick method to re-sum the expenses
        /// </summary>
        private void CalculateExpenses (Assignment assignment)
        {
            assignment.TotalExpenses = expenses.Sum (e => e.Cost);
        }
    }
}

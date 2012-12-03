using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        ExpensePhoto photo;

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
        /// The photo for an expense
        /// </summary>
        public ExpensePhoto Photo
        {
            get { return photo; }
            set { photo = value; OnPropertyChanged("Photo"); }
        }

        /// <summary>
        /// Loads the list of expenses
        /// </summary>
        public Task LoadExpensesAsync (Assignment assignment)
        {
            return service
                .GetExpensesForAssignmentAsync (assignment, CancellationToken.None)
                .ContinueOnUIThread (t => Expenses = t.Result);
        }

        /// <summary>
        /// Saves an expense report
        /// </summary>
        public Task SaveExpenseAsync (Assignment assignment, Expense expense)
        {
            bool newItem = expense.Id == 0;

            return service.SaveExpenseAsync (expense, CancellationToken.None)
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
            return service.DeleteExpenseAsync (expense, CancellationToken.None)
                .ContinueWith (t => {
                    expenses.Remove (expense);
                    CalculateExpenses (assignment);
                });
        }

        /// <summary>
        /// Loads the photo for an expense
        /// </summary>
        public Task LoadPhotoAsync (Expense expense)
        {
            return service
                .GetExpensePhotoAsync (expense, CancellationToken.None)
                .ContinueOnUIThread (t => Photo = t.Result);
        }

        /// <summary>
        /// Saves the photo for an expense
        /// </summary>
        public Task SavePhotoAsync ()
        {
            return service.SaveExpensePhotoAsync (Photo, CancellationToken.None);
        }

        /// <summary>
        /// Just a quick method to re-sum the expenses
        /// </summary>
        private void CalculateExpenses (Assignment assignment)
        {
            if (expenses == null)
                assignment.TotalExpenses = 0;
            else
                assignment.TotalExpenses = expenses.Sum (e => e.Cost);
        }
    }
}

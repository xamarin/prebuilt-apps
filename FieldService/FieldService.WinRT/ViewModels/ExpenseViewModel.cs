using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.WinRT.ViewModels {
    public class ExpenseViewModel : FieldService.ViewModels.ExpenseViewModel {
        DelegateCommand addExpenseCommand, saveExpenseCommand, deleteExpenseCommand, cancelExpenseCommand;

        public ExpenseViewModel ()
        {
            addExpenseCommand = new DelegateCommand (_ => {
            });

            saveExpenseCommand = new DelegateCommand (_ => {
            });

            deleteExpenseCommand = new DelegateCommand (_ => {
            });

            cancelExpenseCommand = new DelegateCommand (_ => {
            });
        }

        /// <summary>
        /// Command to show add expense flyout
        /// </summary>
        public DelegateCommand AddExpenseCommand
        {
            get { return addExpenseCommand; }
        }

        /// <summary>
        /// Command to delete expense 
        /// </summary>
        public DelegateCommand DeleteExpenseCommand
        {
            get { return deleteExpenseCommand; }
        }

        /// <summary>
        /// Command to save expense 
        /// </summary>
        public DelegateCommand SaveExpenseCommand
        {
            get { return saveExpenseCommand; }
        }

        /// <summary>
        /// Command to hide add expense flyout
        /// </summary>
        public DelegateCommand CancelExpenseCommand
        {
            get { return cancelExpenseCommand; }
        }

        /// <summary>
        /// list of top 5 labor hour items
        /// </summary>
        public IEnumerable<Expense> TopExpenses
        {
            get
            {
                if (Expenses == null)
                    return null;

                return Expenses.Take (5);
            }
        }


        protected override void OnPropertyChanged (string propertyName)
        {
            base.OnPropertyChanged (propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "Expenses") {
                OnPropertyChanged ("TopExpenses");
            }
        }
    }
}

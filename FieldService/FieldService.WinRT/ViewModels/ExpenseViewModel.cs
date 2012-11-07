using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;

namespace FieldService.WinRT.ViewModels {
    public class ExpenseViewModel : FieldService.ViewModels.ExpenseViewModel {

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

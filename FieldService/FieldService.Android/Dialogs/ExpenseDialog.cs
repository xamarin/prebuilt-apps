using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android.Dialogs {
    public class ExpenseDialog : BaseDialog {
        ExpenseViewModel expenseViewModel;
        ExpenseCategory [] expenseTypes;
        public ExpenseDialog (Context context)
            : base (context)
        {
            expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();

            expenseTypes = new ExpenseCategory []
            {
                ExpenseCategory.Gas,
                ExpenseCategory.Food,
                ExpenseCategory.Supplies,
                ExpenseCategory.Other,
            };
        }

        protected override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);

            SetContentView (Resource.Layout.AddExpensePopUpLayout);
            SetCancelable (true);
        }

        public Activity Activity
        {
            get;
            set;
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public Expense CurrentExpense
        {
            get;
            set;
        }
    }
}
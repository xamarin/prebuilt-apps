using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FieldService.Android.Fragments;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;

namespace FieldService.Android {
    public class ExpensesAdapter : ArrayAdapter<Expense> {
        List<Expense> expenses;
        ExpenseCategory [] expenseTypes;
        int resourceId;
        ExpenseViewModel expenseViewModel;

        public ExpensesAdapter (Context context, int resourceId, List<Expense> expenses)
            : base (context, resourceId, expenses)
        {
            this.expenses = expenses;
            this.resourceId = resourceId;
            expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();
            expenseTypes = new ExpenseCategory []
            {
                ExpenseCategory.Gas,
                ExpenseCategory.Food,
                ExpenseCategory.Supplies,
                ExpenseCategory.Other,
            };
        }

        public Assignment Assignment
        {
            get;
            set;
        }

        public override View GetView (int position, View convertView, ViewGroup parent)
        {
            Expense expense = null;
            var view = convertView;
            if (view == null) {
                LayoutInflater inflator = (LayoutInflater)Context.GetSystemService (Context.LayoutInflaterService);
                view = inflator.Inflate (resourceId, null);
            }

            if (expenses != null && expenses.Count > position) {
                expense = expenses [position];
            }

            if (expense == null) {
                return view;
            }

            var description = view.FindViewById<TextView> (Resource.Id.expenseDescription);
            var expenseAmount = view.FindViewById<TextView> (Resource.Id.expenseText);
            var expenseType = view.FindViewById<Spinner> (Resource.Id.expenseType);
            var expenseImage = view.FindViewById<ImageView> (Resource.Id.expensePhotoIcon);

            var adapter = new SpinnerAdapter<ExpenseCategory> (expenseTypes, Context, Resource.Layout.SimpleSpinnerItem);
            adapter.TextColor = Color.Black;
            adapter.Background = Color.White;
            expenseType.Adapter = adapter;

            expenseType.ItemSelected += (sender, e) => {
                var status = expenseTypes [e.Position];
                var currentExpense = expenses [position];
                if (status != currentExpense.Category) {
                    currentExpense.Category = status;
                    expenseViewModel.SaveExpense (Assignment, currentExpense).ContinueOnUIThread (_ => {
                        var fragment = ServiceContainer.Resolve<ExpenseFragment> ();
                        fragment.ReloadSingleListItem (position);
                    });
                }
            };

            expenseType.SetSelection (expenseTypes.ToList ().IndexOf (expense.Category));
            expenseAmount.Text = expense.Cost.ToString ("0.00");
            description.Text = expense.Description;

            expenseAmount.Tag = position;

            expenseType.Focusable = false;

            expenseImage.Visibility = expense.Photo != null ? ViewStates.Visible : ViewStates.Invisible;

            return view;
        }
    }
}
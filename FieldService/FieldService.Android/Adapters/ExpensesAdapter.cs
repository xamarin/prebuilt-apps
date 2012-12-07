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
        int resourceId;

        public ExpensesAdapter (Context context, int resourceId, List<Expense> expenses)
            : base (context, resourceId, expenses)
        {
            this.expenses = expenses;
            this.resourceId = resourceId;
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
            var expenseType = view.FindViewById<TextView> (Resource.Id.expenseType);
            var expenseImage = view.FindViewById<ImageView> (Resource.Id.expensePhotoIcon);

            expenseType.Text = expense.CategoryAsString;
            expenseAmount.Text = expense.Cost.ToString ("0.00");
            description.Text = expense.Description;

            expenseAmount.Tag = position;

            expenseImage.Visibility = expense.HasPhoto ? ViewStates.Visible : ViewStates.Invisible;

            return view;
        }
    }
}
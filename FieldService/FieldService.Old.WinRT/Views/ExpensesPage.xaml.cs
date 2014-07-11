//
//  Copyright 2012  Xamarin Inc.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// Page for displaying expenses
    /// </summary>
    public sealed partial class ExpensesPage : Page {
        AssignmentViewModel assignmentViewModel;
        ExpenseViewModel expenseViewModel;

        public ExpensesPage ()
        {
            this.InitializeComponent ();

            DataContext =    
                assignmentControl.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            expensesListView.DataContext =
                expensesAddExpense.DataContext =
                expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();

            Window.Current.SizeChanged += (sender, e) => {
                if (expenseViewModel.ExpensePopUp != null && expenseViewModel.ExpensePopUp.IsOpen) {
                    expenseViewModel.ExpensePopUp.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - Constants.PopUpWidth);
                }
            };
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Invoked when this page is unloaded.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedFrom (NavigationEventArgs e)
        {
            if (expenseViewModel.ExpensePopUp != null && expenseViewModel.ExpensePopUp.IsOpen) {
                expenseViewModel.ExpensePopUp.IsOpen = false;
            }
            base.OnNavigatedFrom (e);
        }


        /// <summary>
        /// Item click for the expense list view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            var expense = e.ClickedItem as Expense;
            if (expense != null) {
                expenseViewModel.AddExpenseCommand.Invoke (expense);
            }
        }
    }
}

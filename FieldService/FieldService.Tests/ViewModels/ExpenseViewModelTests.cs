using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.ViewModels;
using NUnit.Framework;

namespace FieldService.Tests.ViewModels {
    [TestFixture]
    public class ExpenseViewModelTests {

        ExpenseViewModel viewModel;

        [SetUp]
        public void SetUp ()
        {
            ServiceContainer.Register<ISynchronizeInvoke> (() => new Mocks.MockSynchronizeInvoke ());
            ServiceContainer.Register<IAssignmentService> (() => new Mocks.MockAssignmentService ());

            viewModel = new ExpenseViewModel ();
        }

        [Test]
        public void LoadExpenses ()
        {
            var task = viewModel.LoadExpensesAsync (new Assignment ());

            task.Wait ();

            Assert.That (viewModel.Expenses.Count, Is.GreaterThan (0));
        }

        [Test]
        public void SaveExpense ()
        {
            var assignment = new Assignment ();
            var loadTask = viewModel.LoadExpensesAsync (assignment);

            loadTask.Wait ();

            var task = viewModel.SaveExpenseAsync (assignment, new Expense { Cost = 2 });

            task.Wait ();

            Assert.That (assignment.TotalExpenses, Is.EqualTo (2m));
        }

        [Test]
        public void DeleteExpense ()
        {
            var assignment = new Assignment();
            var loadTask = viewModel.LoadExpensesAsync (assignment);

            loadTask.Wait ();
            
            var task = viewModel.DeleteExpenseAsync (assignment, viewModel.Expenses.First());

            task.Wait ();

            Assert.That (assignment.TotalExpenses, Is.EqualTo (0m));
        }
    }
}

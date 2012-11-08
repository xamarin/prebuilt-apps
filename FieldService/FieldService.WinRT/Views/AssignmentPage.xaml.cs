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
using System.Diagnostics;
using FieldService.Data;
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.ViewModels;
using Windows.ApplicationModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;
using Xamarin.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FieldService.WinRT.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssignmentPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly ItemViewModel itemViewModel;
        readonly LaborViewModel laborViewModel;
        readonly PhotoViewModel photoViewModel;
        readonly ExpenseViewModel expenseViewModel;
        readonly DocumentViewModel documentViewModel;
        MediaPicker picker;

        public AssignmentPage ()
        {
            this.InitializeComponent ();

            DataContext =
                assignmentControl.DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            itemsListView.DataContext =
            summaryAddItems.DataContext =
                itemViewModel = ServiceContainer.Resolve<ItemViewModel> ();

            laborListView.DataContext =
                summaryAddLabor.DataContext =
                laborViewModel = ServiceContainer.Resolve<LaborViewModel> ();

            confirmationImage1.DataContext =
                confirmationImage2.DataContext =
                confirmationImage3.DataContext =
                photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();

            expensesListView.DataContext =
                summaryAddExpense.DataContext =
                expenseViewModel = ServiceContainer.Resolve<ExpenseViewModel> ();

            documentsListView.DataContext =
                documentViewModel = ServiceContainer.Resolve<DocumentViewModel> ();

            picker = new MediaPicker ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
            itemViewModel.LoadAssignmentItems (assignmentViewModel.SelectedAssignment);

            laborViewModel.LoadLaborHours (assignmentViewModel.SelectedAssignment);

            photoViewModel.LoadPhotos (assignmentViewModel.SelectedAssignment);

            expenseViewModel.LoadExpenses (assignmentViewModel.SelectedAssignment);

            documentViewModel.LoadDocuments ();
        }

        private void OnItemClick (object sender, ItemClickEventArgs e)
        {
            AssignmentItem item = e.ClickedItem as AssignmentItem;
            if (item != null) {
                item.Used = !item.Used;
                itemViewModel.SaveAssignmentItemCommand.Invoke (item);
            }
        }

        private void OnLaborItemClick (object sender, ItemClickEventArgs e)
        {
            var labor = e.ClickedItem as Labor;
            if (labor != null) {
                laborViewModel.AddLaborCommand.Invoke (labor);
            }
        }

        private void OnExpenseItemClick (object sender, ItemClickEventArgs e)
        {
            var expense = e.ClickedItem as Expense;
            if (expense != null) {
                expenseViewModel.AddExpenseCommand.Invoke (expense);
            }
        }

        private async void OnDocumentItemClick (object sender, ItemClickEventArgs e)
        {
            var document = e.ClickedItem as Document;
            if (document != null) {
                var options = new Windows.System.LauncherOptions { DisplayApplicationPicker = true };
                var file = await Package.Current.InstalledLocation.GetFileAsync (document.Path.Replace('/', '\\'));
                var success = await Windows.System.Launcher.LaunchFileAsync (file, options);
            }
        }

        private async void OnSummaryClick (object sender, ItemClickEventArgs e)
        {
            var element = e.ClickedItem as FrameworkElement;
            switch (element.Name) {
                case "mapTile":
                case "mapButton":
                    Helpers.NavigateTo<AssignmentMapPage> ();
                    break;
                case "totalHours":
                    Helpers.NavigateTo<LaborPage> ();
                    break;
                case "totalItems":
                    Helpers.NavigateTo<ItemsPage> ();
                    break;
                case "confirmation":
                    Helpers.NavigateTo<ConfirmationsPage> ();
                    break;
                case "addSignature":
                    assignmentViewModel.AddSignatureCommand.Invoke ();
                    break;
                case "addImage": {
                        bool cameraCommand = false, imageCommand = false;
                        var dialog = new MessageDialog ("Take picture with your built in camera or select one from your photo library.", "Add Image");
                        if (picker.IsCameraAvailable) {
                            dialog.Commands.Add (new UICommand ("Camera", new UICommandInvokedHandler (_ => cameraCommand = true)));
                        }
                        dialog.Commands.Add (new UICommand ("Library", new UICommandInvokedHandler (_ => imageCommand = true)));

                        await dialog.ShowAsync ();

                        if (cameraCommand) {
                            StoreCameraMediaOptions options = new StoreCameraMediaOptions {
                                Directory = "FieldService",
                                Name = "FieldService.jpg",
                            };
                            try{
                            var mediaFile = await picker.TakePhotoAsync (options);

                            var photo = new Photo ();
                            await mediaFile.GetStream ().LoadBytes ().ContinueWith (t => {
                                photo.Image = t.Result;
                            });
                            photoViewModel.PhotoSelectedCommand.Invoke (photo);
                            Helpers.NavigateTo<ImagesPage> ();
                            } catch (Exception exc) {
                                Debug.WriteLine (exc.Message);
                                //this could happen if they cancel, etc.
                            }
                        } else if (imageCommand) {
                            try{
                            var mediaFile = await picker.PickPhotoAsync ();

                            var photo = new Photo ();
                            await mediaFile.GetStream ().LoadBytes ().ContinueWith (t => {
                                photo.Image = t.Result;
                            });
                            photoViewModel.PhotoSelectedCommand.Invoke (photo);
                            Helpers.NavigateTo<ImagesPage> ();
                            } catch (Exception exc) {
                                Debug.WriteLine (exc.Message);
                                //this could happen if they cancel, etc.
                            }
                        }
                    }
                    break;
                case "confirmationImage1": {
                        var photo = photoViewModel.FirstImage;
                        if (photo != null) {
                            photoViewModel.PhotoSelectedCommand.Invoke (photo);
                            Helpers.NavigateTo<ImagesPage> ();
                        }
                    }
                    break;
                case "confirmationImage2": {
                        var photo = photoViewModel.SecondImage;
                        if (photo != null) {
                            photoViewModel.PhotoSelectedCommand.Invoke (photo);
                            Helpers.NavigateTo<ImagesPage> ();
                        }
                    }
                    break;
                case "confirmationImage3": {
                        var photo = photoViewModel.ThirdImage;
                        if (photo != null) {
                            photoViewModel.PhotoSelectedCommand.Invoke (photo);
                            Helpers.NavigateTo<ImagesPage> ();
                        }
                    }
                    break;
                case "expenses":
                    Helpers.NavigateTo<ExpensesPage> ();
                    break;
                case "documents":
                    Helpers.NavigateTo<DocumentsPage> ();
                    break;
                default:
                    await new MessageDialog ("Coming soon!").ShowAsync ();
                    break;
            }
        }
    }
}

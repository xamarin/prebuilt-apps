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

using FieldService.Data;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FieldService.WinRT.ViewModels {
    public class AssignmentViewModel : FieldService.ViewModels.AssignmentViewModel {
        readonly DelegateCommand recordCommand, mapsCommand, goBackCommand, itemsCommand, laborCommand, confirmationsCommand, cancelAddSignatureCommand,
            addSignatureCommand, expensesCommand, documentsCommand, historyCommand, declineCommand, acceptCommand;
        Assignment assignment;
        Popup addSignaturePopup;
        int popUpWidth = 930;

        public AssignmentViewModel ()
        {
            recordCommand = new DelegateCommand (async _ => {
                if (Recording)
                    await PauseAsync ();
                else
                    await RecordAsync ();
            }, _ => !IsBusy);

            mapsCommand = new DelegateCommand (_ => Helpers.NavigateTo<MapPage> ());

            goBackCommand = new DelegateCommand (_ => {
                if (addSignaturePopup != null && addSignaturePopup.IsOpen) {
                    addSignaturePopup.IsOpen = false;
                }
                    Helpers.GoBack ();
                }, _ => Helpers.CanGoBack);

            itemsCommand = new DelegateCommand (_ => Helpers.NavigateTo<ItemsPage> ());

            laborCommand = new DelegateCommand (_ => Helpers.NavigateTo<LaborPage> ());

            expensesCommand = new DelegateCommand (_ => Helpers.NavigateTo<ExpensesPage> ());

            confirmationsCommand = new DelegateCommand (_ => Helpers.NavigateTo<ConfirmationsPage> ());

            documentsCommand = new DelegateCommand (_ => Helpers.NavigateTo<DocumentsPage> ());

            historyCommand = new DelegateCommand (_ => Helpers.NavigateTo<AssignmentHistoryPage> ());

            cancelAddSignatureCommand = new DelegateCommand (_ => { addSignaturePopup.IsOpen = false; });

            declineCommand = new DelegateCommand (async _ => {
                SelectedAssignment.Status = AssignmentStatus.Declined;
                await SaveAssignmentAsync (SelectedAssignment);
                Helpers.GoBack ();
            });

            acceptCommand = new DelegateCommand (async _ => {
                if (ActiveAssignment == null) {
                    SelectedAssignment.Status = AssignmentStatus.Active;
                } else {
                    SelectedAssignment.Status = AssignmentStatus.Hold;
                }
                await SaveAssignmentAsync (SelectedAssignment);
                SelectedAssignment = SelectedAssignment;
            });

            addSignatureCommand = new DelegateCommand (_ => {
                if (addSignaturePopup == null) {
                    addSignaturePopup = new Popup ();
                    addSignaturePopup.Height = Window.Current.Bounds.Height;
                    addSignaturePopup.Width = popUpWidth;
                    AddSignatureFlyoutPanel flyoutpanel = new AddSignatureFlyoutPanel ();
                    flyoutpanel.Width = addSignaturePopup.Width;
                    flyoutpanel.Height = addSignaturePopup.Height;
                    addSignaturePopup.Child = flyoutpanel;
                    addSignaturePopup.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - popUpWidth);
                    addSignaturePopup.SetValue (Canvas.TopProperty, 0);
                }
                addSignaturePopup.IsOpen = true;
            });
        }

        /// <summary>
        /// Hours formatted for WinRT
        /// </summary>
        public string HoursFormatted
        {
            get { return string.Format ("{0}h {1}m {2}s", Hours.Hours, Hours.Minutes, Hours.Seconds); }
        }

        /// <summary>
        /// Selected assignment
        /// </summary>
        public Assignment SelectedAssignment
        {
            get { return assignment; }
            set
            {
                assignment = value;
                OnPropertyChanged ("SelectedAssignment");
                OnPropertyChanged ("IsNew");
                OnPropertyChanged ("IsNotNew");
            }
        }

        /// <summary>
        /// Command for the record button
        /// </summary>
        public DelegateCommand RecordCommand
        {
            get { return recordCommand; }
        }

        /// <summary>
        /// Command for navigating to the maps page
        /// </summary>
        public DelegateCommand MapsCommand
        {
            get { return mapsCommand; }
        }

        /// <summary>
        /// Command for navigating back
        /// </summary>
        public DelegateCommand GoBackCommand
        {
            get { return goBackCommand; }
        }

        /// <summary>
        /// Command for going to the items page
        /// </summary>
        public DelegateCommand ItemsCommand
        {
            get { return itemsCommand; }
        }

        /// <summary>
        /// Command for going to the labor page
        /// </summary>
        public DelegateCommand LaborCommand
        {
            get { return laborCommand; }
        }

        /// <summary>
        /// Command for going to the confirmations page
        /// </summary>
        public DelegateCommand ConfirmationsCommand
        {
            get { return confirmationsCommand; }
        }

        /// <summary>
        /// Command for cancel the add signature flyout.
        /// </summary>
        public DelegateCommand CancelAddSignatureCommand
        {
            get { return cancelAddSignatureCommand; }
        }

        /// <summary>
        /// Command for opening signature flyout.
        /// </summary>
        public DelegateCommand AddSignatureCommand
        {
            get { return addSignatureCommand; }
        }

        /// <summary>
        /// Command for going to the expenses page
        /// </summary>
        public DelegateCommand ExpensesCommand
        {
            get { return expensesCommand; }
        }

        /// <summary>
        /// Command for going to the documents page
        /// </summary>
        public DelegateCommand DocumentsCommand
        {
            get { return documentsCommand; }
        }

        /// <summary>
        /// Command for going to the documents page
        /// </summary>
        public DelegateCommand HistoryCommand
        {
            get { return historyCommand; }
        }

        /// <summary>
        /// Command for accepting assignments
        /// </summary>
        public DelegateCommand AcceptAssignmentCommand
        {
            get { return acceptCommand; }
        }

        /// <summary>
        /// Command for declining assignments
        /// </summary>
        public DelegateCommand DeclineAssignmentCommand
        {
            get { return declineCommand; }
        }

        protected override void OnIsBusyChanged ()
        {
            base.OnIsBusyChanged ();

            if (recordCommand != null)
                recordCommand.RaiseCanExecuteChanged ();
            if (goBackCommand != null)
                goBackCommand.RaiseCanExecuteChanged ();
        }

        /// <summary>
        /// Value for showing accept/decline
        /// </summary>
        public bool IsNew
        {
            get { return SelectedAssignment != null && SelectedAssignment.Status == AssignmentStatus.New; }
        }

        /// <summary>
        /// Value for showing combo box
        /// </summary>
        public bool IsNotNew
        {
            get { return SelectedAssignment != null && SelectedAssignment.Status != AssignmentStatus.New; }
        }

        /// <summary>
        /// Value for showing complete checkmark
        /// </summary>
        public bool IsComplete
        {
            get { return SelectedAssignment != null && SelectedAssignment.Status == AssignmentStatus.Complete; }
        }

        protected override void OnHoursChanged ()
        {
            base.OnHoursChanged ();

            OnPropertyChanged ("HoursFormatted");
        }
    }
}

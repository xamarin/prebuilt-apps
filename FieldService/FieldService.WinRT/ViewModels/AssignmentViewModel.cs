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
using FieldService.Utilities;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace FieldService.WinRT.ViewModels {
    public class AssignmentViewModel : FieldService.ViewModels.AssignmentViewModel {
        readonly DelegateCommand recordCommand, mapsCommand, assignmentMapsCommand, goBackCommand, itemsCommand, laborCommand, confirmationsCommand, cancelAddSignatureCommand,
            addSignatureCommand, expensesCommand, documentsCommand, historyCommand, declineCommand, acceptCommand, goBackHistoryCommand, completeCommand;
        Assignment previousAssignment;
        Popup addSignaturePopup;

        public AssignmentViewModel ()
        {
            recordCommand = new DelegateCommand (async _ => {
                if (Recording)
                    await PauseAsync ();
                else
                    await RecordAsync ();
            }, _ => !IsBusy);

            mapsCommand = new DelegateCommand (_ => Helpers.NavigateTo<MapPage> ());

            assignmentMapsCommand = new DelegateCommand (_ => Helpers.NavigateTo<AssignmentMapPage> ());

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

            goBackHistoryCommand = new DelegateCommand (_ => {
                Helpers.GoBack ();
                if (SelectedAssignment.IsHistory) {
                    SelectedAssignment = PreviousSelected;
                    PreviousSelected = null;
                }
            });

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

                //Reload the entire page
                Helpers.GoBack();
                Helpers.NavigateTo<AssignmentPage>();
            });

            addSignatureCommand = new DelegateCommand (_ => {
                if (addSignaturePopup != null && addSignaturePopup.IsOpen) {
                    addSignaturePopup.IsOpen = false;
                }
                addSignaturePopup = new Popup ();
                addSignaturePopup.Height = Window.Current.Bounds.Height;
                addSignaturePopup.Width = Constants.SignaturePopUpWidth;
                AddSignatureFlyoutPanel flyoutpanel = new AddSignatureFlyoutPanel ();
                flyoutpanel.Width = addSignaturePopup.Width;
                flyoutpanel.Height = addSignaturePopup.Height;
                addSignaturePopup.Child = flyoutpanel;
                addSignaturePopup.SetValue (Canvas.LeftProperty, Window.Current.Bounds.Width - Constants.SignaturePopUpWidth);
                addSignaturePopup.SetValue (Canvas.TopProperty, 0);
                addSignaturePopup.IsOpen = true;
            });

            completeCommand = new DelegateCommand (async _ => {
                SelectedAssignment.Status = AssignmentStatus.Complete;
                await SaveAssignmentAsync (SelectedAssignment);

                //Reload the entire page
                Helpers.GoBack ();
                Helpers.NavigateTo<ConfirmationsPage> ();
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
        public override Assignment SelectedAssignment
        {
            get { return base.SelectedAssignment; }
            set
            {
                base.SelectedAssignment = value;
                OnPropertyChanged ("IsNew");
                OnPropertyChanged ("IsNotNew");
                OnPropertyChanged ("AssignmentTitle");
                OnPropertyChanged ("BackgroundImage");
            }
        }

        public Assignment PreviousSelected
        {
            get { return previousAssignment; }
            set { previousAssignment = value; OnPropertyChanged ("PreviousSelected"); }
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
        /// Command for navigating to the maps page for a specific assignment
        /// </summary>
        public DelegateCommand AssignmentMapsCommand
        {
            get { return assignmentMapsCommand; }
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

        /// <summary>
        /// Command for navigating back from history screen
        /// </summary>
        public DelegateCommand GoBackHistoryCommand
        {
            get { return goBackHistoryCommand; }
        }

        /// <summary>
        /// Command for navigating back from history screen
        /// </summary>
        public DelegateCommand CompleteCommand
        {
            get { return completeCommand; }
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
        /// Value for showing timer control
        /// </summary>
        public bool IsTimerVisible
        {
            get { return SelectedAssignment != null && SelectedAssignment.Status == AssignmentStatus.Active; }
        }

        /// <summary>
        /// Value for showing complete checkmark
        /// </summary>
        public bool IsComplete
        {
            get { return SelectedAssignment != null && SelectedAssignment.Status == AssignmentStatus.Complete; }
        }

        /// <summary>
        /// Value for showing complete text
        /// </summary>
        public string IsCompleteText
        {
            get { return IsComplete ? "Complete" : "Mark Complete"; }
        }

        /// <summary>
        /// Value for not readonly 
        /// </summary>
        public bool IsNotReadonly
        {
            get { return !IsReadonly; }
        }

        /// <summary>
        /// Value for readonly 
        /// </summary>
        public bool IsReadonly
        {
            get { return SelectedAssignment.IsHistory || IsComplete; }
        }

        /// <summary>
        /// Value for showing correct brush color
        /// </summary>
        public SolidColorBrush IsCompleteColor
        {
            get
            {
                var app = ServiceContainer.Resolve<App> ();
                if (IsComplete) {
                    return app.Resources ["DarkTextBrush"] as SolidColorBrush;
                }
                return app.Resources ["LightItemsBrush"] as SolidColorBrush;
            }
        }

        /// <summary>
        /// Value for showing correct brush color
        /// </summary>
        public SolidColorBrush IsCompleteBottomColor
        {
            get
            {
                var app = ServiceContainer.Resolve<App> ();
                if (IsComplete) {
                    return app.Resources ["DarkBottomTextBrush"] as SolidColorBrush;
                }
                return app.Resources ["DarkBlueItemsBrush"] as SolidColorBrush;
            }
        }

        /// <summary>
        /// value for showing background image
        /// </summary>
        public BitmapImage BackgroundImage
        {
            get
            {
                var app = ServiceContainer.Resolve<App> ();
                if (SelectedAssignment.IsHistory) {
                    return app.Resources ["HistoryBackgroundImage"] as BitmapImage;
                }
                return app.Resources ["BackgroundImage"] as BitmapImage;
            }
        }

        /// <summary>
        /// Value for changing assignment title based on is history
        /// </summary>
        public string AssignmentTitle
        {
            get
            {
                if (SelectedAssignment.IsHistory) {
                    return string.Format ("History for {0}", SelectedAssignment.JobNumberFormatted);
                }
                return SelectedAssignment.JobNumberFormatted;
            }
        }

        /// <summary>
        /// signature pop up
        /// </summary>
        public Popup SignaturePopUp
        {
            get { return addSignaturePopup; }
        }

        /// <summary>
        /// Value for showing total accumulated time
        /// </summary>
        public string TotalAccumulated
        {
            get { return string.Format ("Total {0}:{1}", Hours.Hours.ToString ("#0"), Hours.Minutes.ToString ("00")); }
        }

        protected override void OnHoursChanged ()
        {
            base.OnHoursChanged ();

            OnPropertyChanged ("HoursFormatted");
            OnPropertyChanged ("TotalAccumulated");
        }
    }
}
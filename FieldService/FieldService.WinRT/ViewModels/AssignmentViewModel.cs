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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.WinRT.Utilities;
using FieldService.WinRT.Views;

namespace FieldService.WinRT.ViewModels {
    public class AssignmentViewModel : FieldService.ViewModels.AssignmentViewModel {
        readonly DelegateCommand recordCommand, mapsCommand, goBackCommand, itemsCommand, laborCommand;
        Assignment assignment;

        public AssignmentViewModel ()
        {
            recordCommand = new DelegateCommand (async _ => {
                if (Recording)
                    await Pause ();
                else
                    await Record ();
            }, _ => !IsBusy);

            mapsCommand = new DelegateCommand (_ => Helpers.NavigateTo<MapPage> ());

            goBackCommand = new DelegateCommand (_ => Helpers.GoBack (), _ => Helpers.CanGoBack);

            itemsCommand = new DelegateCommand (_ => Helpers.NavigateTo<ItemsPage> ());

            laborCommand = new DelegateCommand (_ => Helpers.NavigateTo<LaborPage> ());
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
            set { assignment = value; OnPropertyChanged ("SelectedAssignment"); }
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

        protected override void OnIsBusyChanged ()
        {
            base.OnIsBusyChanged ();

            if (recordCommand != null)
                recordCommand.RaiseCanExecuteChanged ();
            if (goBackCommand != null)
                goBackCommand.RaiseCanExecuteChanged ();
        }

        protected override void OnHoursChanged ()
        {
            base.OnHoursChanged ();

            OnPropertyChanged ("HoursFormatted");
        }
    }
}

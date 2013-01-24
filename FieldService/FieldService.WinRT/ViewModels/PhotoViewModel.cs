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
using FieldService.Data;
using FieldService.Utilities;
using Windows.UI.Popups;

namespace FieldService.WinRT.ViewModels {
    public class PhotoViewModel : FieldService.ViewModels.PhotoViewModel {
        readonly DelegateCommand photoSelectedCommand, savePhotoCommand, deletePhotoCommand;
        readonly AssignmentViewModel assignmentViewModel;
        Photo selectedPhoto;

        public PhotoViewModel ()
        {
            assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            photoSelectedCommand = new DelegateCommand (obj => {
                var photo = obj as Photo;
                if (photo != null) {
                    SelectedPhoto = photo;
                } else
                    SelectedPhoto = null;
            });

            savePhotoCommand = new DelegateCommand (async _ => {
                selectedPhoto.AssignmentId = assignmentViewModel.SelectedAssignment.Id;
                await SavePhotoAsync (assignmentViewModel.SelectedAssignment, selectedPhoto);
                await LoadPhotosAsync (assignmentViewModel.SelectedAssignment);
                OnPropertyChanged ("Photos");
            });

            deletePhotoCommand = new DelegateCommand (async _ => {
                bool yesDelete = false;
                var dialog = new MessageDialog ("Are you sure?", "Delete Image");
                dialog.Commands.Add (new UICommand ("Yes", del => { yesDelete = true; }));
                dialog.Commands.Add(new UICommand("No"));
                await dialog.ShowAsync ();
                if (yesDelete) {
                    await DeletePhotoAsync (assignmentViewModel.SelectedAssignment, selectedPhoto);
                    await LoadPhotosAsync (assignmentViewModel.SelectedAssignment);
                    SelectedPhoto = null;
                }
            });
        }

        /// <summary>
        /// Photo selected command
        /// </summary>
        public DelegateCommand PhotoSelectedCommand
        {
            get { return photoSelectedCommand; }
        }

        /// <summary>
        /// Saving photo command
        /// </summary>
        public DelegateCommand SavePhotoCommand
        {
            get { return savePhotoCommand; }
        }

        /// <summary>
        /// Deleting photo command
        /// </summary>
        public DelegateCommand DeletePhotoCommand
        {
            get { return deletePhotoCommand; }
        }

        public override Photo SelectedPhoto
        {
            get { return selectedPhoto; }
            set
            {
                selectedPhoto = value;
                OnPropertyChanged ("SelectedPhoto");
                OnPropertyChanged ("CanDelete");
                OnPropertyChanged ("CanSave");
            }
        }

        /// <summary>
        /// bool value for showing delete button
        /// </summary>
        public bool CanDelete
        {
            get { return SelectedPhoto != null; }
        }

        /// <summary>
        /// bool value for showing save button
        /// </summary>
        public bool CanSave
        {
            get { return SelectedPhoto != null; }
        }

        /// <summary>
        /// Value for not is history
        /// </summary>
        public bool IsNotReadonly
        {
            get { return assignmentViewModel.IsNotReadonly; }
        }

        protected override void OnPropertyChanged (string propertyName)
        {
            base.OnPropertyChanged (propertyName);

            //Make sure property changed is raised for new properties
            if (propertyName == "Photos") {
                OnPropertyChanged ("TopPhotos");
                OnPropertyChanged ("FirstImage");
                OnPropertyChanged ("SecondImage");
                OnPropertyChanged ("ThirdImage");
            }
        }
    }
}

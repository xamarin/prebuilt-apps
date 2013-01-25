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
using System.Threading;
using System.Threading.Tasks;
using FieldService.Data;
using FieldService.Utilities;

namespace FieldService.ViewModels {
    /// <summary>
    /// ViewModel for working with photos
    /// </summary>
    public class PhotoViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<Photo> photos;
	Photo selectedPhoto;

        public PhotoViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of photos for an assignment
        /// </summary>
        public List<Photo> Photos
        {
            get { return photos; }
            set { photos = value; OnPropertyChanged ("AssignmentItems"); OnPropertyChanged ("Photos"); }
        }

	/// <summary>
	/// The selected photo for use within the app
	/// </summary>
	public virtual Photo SelectedPhoto
	{
	    get { return selectedPhoto; }
	    set { selectedPhoto = value; OnPropertyChanged ("SelectedPhoto"); }
	}

        /// <summary>
        /// Loads all the photos for an assignment
        /// </summary>
        public Task LoadPhotosAsync (Assignment assignment)
        {
            return service
                .GetPhotosForAssignmentAsync (assignment)
                .ContinueOnCurrentThread (t => Photos = t.Result);
        }

        /// <summary>
        /// Saves a photo
        /// </summary>
        public Task SavePhotoAsync (Assignment assignment, Photo photo)
        {
            bool newPhoto = photo.Id == 0;

            //Update the date
            photo.Date = DateTime.Now;

            return service
                .SavePhotoAsync (photo)
                .ContinueOnCurrentThread (t => {
                    if (newPhoto) {
                        if (photos == null)
                            photos = new List<Photo> ();
                        photos.Add (photo);
                        OnPropertyChanged ("Photos");
                    }
                });
        }

        /// <summary>
        /// Deletes a photo
        /// </summary>
        public Task DeletePhotoAsync (Assignment assignment, Photo photo)
        {
            return service
                .DeletePhotoAsync (photo)
                .ContinueOnCurrentThread (t => {
                    if (photos != null)
                        photos.Remove (photo);
                });
        }
    }
}

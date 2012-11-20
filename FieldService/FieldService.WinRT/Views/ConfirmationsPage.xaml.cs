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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Xamarin.Media;

namespace FieldService.WinRT.Views {
    /// <summary>
    /// Confirmations page for 
    /// </summary>
    public sealed partial class ConfirmationsPage : Page {
        readonly AssignmentViewModel assignmentViewModel;
        readonly PhotoViewModel photoViewModel;
        MediaPicker picker;
        public ConfirmationsPage ()
        {
            this.InitializeComponent ();

            assignmentControl.DataContext =
                DataContext =
                assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel> ();

            photoListView.DataContext =
                photoViewModel = ServiceContainer.Resolve<PhotoViewModel> ();

            picker = new MediaPicker ();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo (NavigationEventArgs e)
        {
        }
        
        private async void OnItemClick (object sender, ItemClickEventArgs e)
        {
            var element = e.ClickedItem as FrameworkElement;
            switch (element.Name) {
                case "addSignature":
                    assignmentViewModel.AddSignatureCommand.Invoke ();
                    break;
                case "markComplete":
                    if (assignmentViewModel.Signature == null) {
                        await new MessageDialog ("No signature!").ShowAsync ();
                    }
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
                            try {
                                var mediaFile = await picker.TakePhotoAsync (options);

                                var photo = new Photo ();
                                await mediaFile.GetStream ().LoadBytes ().ContinueWith (t => {
                                    photo.Image = t.Result;
                                });
                                photoViewModel.PhotoSelectedCommand.Invoke (photo);
                                Helpers.NavigateTo<ImagesPage> ();
                            } catch(Exception exc) {
                                Debug.WriteLine (exc.Message);
                                //this could happen if they cancel, etc.
                            }
                        } else if (imageCommand) {
                            try {
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
                default:
                    await new MessageDialog ("Coming soon!").ShowAsync ();
                    break;
            }
        }

        private void OnImageClick (object sender, ItemClickEventArgs e)
        {
            var photo = e.ClickedItem as Photo;
            if (photo != null) {
                photoViewModel.PhotoSelectedCommand.Invoke (photo);
                Helpers.NavigateTo<ImagesPage> ();
            }
        }
    }
}

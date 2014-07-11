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
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using FieldService.ViewModels;
using FieldService.Utilities;
using System.IO;

namespace FieldService.iOS
{
	public partial class DocumentController : BaseController
	{
		readonly DocumentViewModel documentViewModel;
		readonly AssignmentViewModel assignmentViewModel;
		UILabel title;

		public DocumentController (IntPtr handle) : base (handle)
		{
			documentViewModel = ServiceContainer.Resolve<DocumentViewModel>();
			assignmentViewModel = ServiceContainer.Resolve<AssignmentViewModel>();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			//UI to setup from code
			View.BackgroundColor = Theme.BackgroundColor;
			title = new UILabel (new RectangleF (0, 0, 160, 36)) { 
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
				Font = Theme.BoldFontOfSize (16),
			};
			var titleButton = new UIBarButtonItem (title);
			
			toolbar.Items = new UIBarButtonItem[] { titleButton };

			tableView.Source = new TableSource (documentViewModel);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			ReloadDocuments ();
		}

		/// <summary>
		/// Reload the documents
		/// </summary>
		public void ReloadDocuments ()
		{
			if (IsViewLoaded) {
				toolbar.SetBackgroundImage (assignmentViewModel.SelectedAssignment.IsHistory ? Theme.OrangeBar : Theme.BlueBar, UIToolbarPosition.Any, UIBarMetrics.Default);

				documentViewModel.LoadDocumentsAsync ()
					.ContinueWith (_ => {
						BeginInvokeOnMainThread (() => {
							if (documentViewModel.Documents == null || documentViewModel.Documents.Count == 0) 
								title.Text = "Documents";
							else
								title.Text = string.Format ("Documents ({0})", documentViewModel.Documents.Count);
							tableView.ReloadData ();
						});
					});
			}
		}

		/// <summary>
		/// Table source for expenses
		/// </summary>
		private class TableSource : UITableViewSource
		{
			readonly DocumentViewModel documentViewModel;
			UIDocumentInteractionController documentController;
			const string Identifier = "DocumentCell";

			public TableSource (DocumentViewModel documentViewModel)
			{
				this.documentViewModel = documentViewModel;
				documentController = new UIDocumentInteractionController();
			}

			public override int RowsInSection (UITableView tableview, int section)
			{
				return documentViewModel.Documents == null ? 0 : documentViewModel.Documents.Count;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.CellAt (indexPath) as DocumentCell;
				cell.Clicked ();
			}

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				var document = documentViewModel.Documents [indexPath.Row];
				var cell = tableView.DequeueReusableCell (Identifier) as DocumentCell;
				if (cell == null) {
					cell = new DocumentCell(Identifier);
					cell.DocumentController = documentController;
				}
				cell.SetDocument (document);
				return cell;
			}
		}
	}
}

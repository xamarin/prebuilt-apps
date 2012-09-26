using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeDirectory.Data;
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;

namespace EmployeeDirectory.ViewModels
{
	public class SearchViewModel
	{
		IDirectoryService service;
		Search search;

		public SearchViewModel (IDirectoryService service, Search search)
		{
			if (service == null) throw new ArgumentNullException ("service");
			this.service = service;

			if (search == null) throw new ArgumentNullException ("search");
			this.search = search;

			SetGroupedPeople ();
		}

		#region View Data

		public string Title {
			get {
				if (string.IsNullOrEmpty (search.Name)) {
					return "Search";
				}
				else {
					return search.Name;
				}
			}
		}

		public ObservableCollection<PeopleGroup> GroupedPeople { get; private set; }

		public SearchProperty SearchProperty {
			get { return search.Property; }
			set { search.Property = value; }
		}

		public string SearchText {
			get { return search.Text; }
			set { search.Text = value.Trim (); }
		}

		#endregion


		#region Commands

		CancellationTokenSource lastCancelSource = null;

		public void Search ()
		{
			//
			// Stop previous search
			//
			if (lastCancelSource != null) {
				lastCancelSource.Cancel ();
			}

			//
			// Perform the search
			//
			var tokenSource = new CancellationTokenSource ();
			var token = tokenSource.Token;
			service.SearchAsync (search.Filter, 200).ContinueWith (searchTask => {

					if (searchTask.IsFaulted) {
						var ev = Error;
						if (ev != null) {
							ev (this, new ErrorEventArgs (searchTask.Exception));
						}
					}
					else {
						search.Results = searchTask.Result.ToList ();
						SetGroupedPeople ();
						search.SaveAsync ();

						var ev = SearchCompleted;
						if (ev != null && !token.IsCancellationRequested) {
							ev (this, EventArgs.Empty);
						}
					}

				},
				token,
				TaskContinuationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext ());

			lastCancelSource = tokenSource;
		}

		/// <summary>
		/// Groups people by the initial letter of their last name
		/// </summary>
		void SetGroupedPeople ()
		{
			GroupedPeople = PeopleGroup.CreateGroups (search.Results);
		}

		#endregion

		#region Events

		public event EventHandler<ErrorEventArgs> Error;

		public event EventHandler SearchCompleted;

		#endregion
	}


}

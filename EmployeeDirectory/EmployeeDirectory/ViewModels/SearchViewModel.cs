using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmployeeDirectory.Data;
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;

namespace EmployeeDirectory.ViewModels
{
	public class SearchViewModel : ViewModel
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

		public ObservableCollection<PeopleGroup> Groups { get; private set; }

		public SearchProperty SearchProperty {
			get { return search.Property; }
			set { search.Property = value; }
		}

		public string SearchText {
			get { return search.Text; }
			set { search.Text = value ?? ""; }
		}

		bool groupByLastName = true;
		public bool GroupByLastName {
			get { return groupByLastName; }
			set {
				if (groupByLastName != value) {
					groupByLastName = value;
					SetGroupedPeople ();
				}
			}
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
			lastCancelSource = new CancellationTokenSource ();
			var token = lastCancelSource.Token;
			service.SearchAsync (search.Filter, 200).ContinueWith (
				OnSearchCompleted,
				token,
				TaskContinuationOptions.None,
				TaskScheduler.FromCurrentSynchronizationContext ());
		}

		void OnSearchCompleted (Task<IList<Person>> searchTask)
		{
			if (searchTask.IsFaulted) {
				var ev = Error;
				if (ev != null) {
					ev (this, new ErrorEventArgs (searchTask.Exception));
				}
			}
			else {
				search.Results = new Collection<Person> (searchTask.Result);
				search.Save ();
				SetGroupedPeople ();				

				var ev = SearchCompleted;
				if (ev != null) {
					ev (this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Groups people by the initial letter of their last name
		/// </summary>
		void SetGroupedPeople ()
		{
			Groups = PeopleGroup.CreateGroups (search.Results, groupByLastName);
			OnPropertyChanged ("Groups");
		}

		#endregion

		#region Events

		public event EventHandler<ErrorEventArgs> Error;

		public event EventHandler SearchCompleted;

		#endregion
	}


}

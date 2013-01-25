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
    /// View model for labor hours
    /// </summary>
    public class LaborViewModel : ViewModelBase {
        readonly IAssignmentService service;
        List<Labor> laborHours;
	Labor selectedLabor;

        public LaborViewModel ()
        {
            service = ServiceContainer.Resolve<IAssignmentService> ();
        }

        /// <summary>
        /// List of labor hours
        /// </summary>
        public List<Labor> LaborHours
        {
            get { return laborHours; }
            set { laborHours = value; OnPropertyChanged ("LaborHours"); }
        }

	/// <summary>
	/// The selected labor hour for use within the app
	/// </summary>
        public virtual Labor SelectedLabor
	{
	    get { return selectedLabor; }
	    set { selectedLabor = value; OnPropertyChanged ("SelectedLabor"); }
	}

        /// <summary>
        /// Loads the list of labor hours
        /// </summary>
        public Task LoadLaborHoursAsync (Assignment assignment)
        {
            return service
                .GetLaborForAssignmentAsync (assignment)
                .ContinueOnCurrentThread (t => LaborHours = t.Result);
        }

        /// <summary>
        /// Saves a labor entry
        /// </summary>
        public Task SaveLaborAsync (Assignment assignment, Labor labor)
        {
            bool newItem = labor.Id == 0;

            return service.SaveLaborAsync (labor)
                .ContinueWith (t => {
                    if (newItem)
                        laborHours.Add (labor);
                    CalculateHours (assignment);
                });
        }

        /// <summary>
        /// Deletes a labor entry
        /// </summary>
        public Task DeleteLaborAsync (Assignment assignment, Labor labor)
        {
            return service.DeleteLaborAsync (labor)
                .ContinueWith (t => {
                    laborHours.Remove (labor);
                    CalculateHours (assignment);
                });
        }

        /// <summary>
        /// Just a quick method to re-sum the hours
        /// </summary>
        private void CalculateHours (Assignment assignment)
        {
            if (laborHours == null)
                assignment.TotalHours = TimeSpan.Zero;
            else
                assignment.TotalHours = TimeSpan.FromMilliseconds (laborHours.Sum (l => l.Hours.TotalMilliseconds));
        }
    }
}